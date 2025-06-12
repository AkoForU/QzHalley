using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Main.Models;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core services
builder.Services.AddDbContext<QuizDbContext>();
builder.Services.AddLogging(logging => logging.AddConsole());

var app = builder.Build();

// In-memory message store
List<Message> messages = new();

// Get and display the local IP address
string GetLocalIPAddress()
{
    foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
    {
        if (networkInterface.OperationalStatus == OperationalStatus.Up &&
            networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        {
            foreach (var unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return unicastAddress.Address.ToString();
                }
            }
        }
    }
    return "Unknown";
}
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation($"Server starting... Local IP Address: {GetLocalIPAddress()}");

// Serve static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "images")),
    RequestPath = "/images"
});

// Map API endpoints
app.MapPost("/send_message", async (HttpContext context) =>
{
    logger.LogInformation($"Received POST request for /send_message");
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    logger.LogInformation($"Request Body: {requestBody}");
    try
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var message = JsonSerializer.Deserialize<Message>(requestBody, options);
        if (string.IsNullOrEmpty(message?.Sender) || string.IsNullOrEmpty(message?.Content))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("{\"error\":\"Missing sender or message\"}");
            return;
        }
        messages.Add(message);
        if (message.Content.StartsWith("image:"))
        {
            string imageName = message.Content.Replace("image:", "").Trim();
            messages.Add(new Message { Sender = "Server", Content = $"Image {imageName} requested" });
        }
        else
        {
            messages.Add(new Message { Sender = "Server", Content = $"Received: {message.Content}" });
        }
        context.Response.StatusCode = 200;
    }
    catch (JsonException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("{\"error\":\"Invalid JSON: " + ex.Message + "\"}");
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("{\"error\":\"Server error: " + ex.Message + "\"}");
    }
});

app.MapGet("/get_messages", (HttpContext context) =>
{
    if (context.Request.Query["sender"] == "guest")
    {
        return Results.Json(new Message { Sender = "Server", Content = "Welcome onboard" });
    }
    if (context.Request.Query["sender"]=="admin")
    {  
        string sender = context.Request.Query["sender"];
        var filteredMessages = string.IsNullOrEmpty(sender)
            ? messages
            : messages.ToList();
        return Results.Json(filteredMessages);
    }
    return Results.Json(new Message { Sender = "Server", Content = "No access for you" });

});

app.MapPost("/authenticate", async (HttpContext context, QuizDbContext dbContext) =>
{
    try
    {
        var form = await context.Request.ReadFormAsync();
        string username = form["username"];
        string password = form["password"];

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new AuthResult { Success = false, UserId = 0 }));
            return;
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == username && u.Password == password);

        var authResult = new AuthResult
        {
            Success = user != null,
            UserId = user?.Id ?? 0
        };

        context.Response.StatusCode = 200;
        await context.Response.WriteAsync(JsonSerializer.Serialize(authResult));
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new AuthResult { Success = false, UserId = 0 }));
        logger.LogInformation($"Authentication error: {ex.Message}");
    }
});

app.MapGet("/users", async (HttpContext context, QuizDbContext dbContext) =>
{
    var users = await dbContext.Users.Select(u => new { u.Id, u.Name }).ToListAsync();
    return Results.Json(users);
});

app.MapGet("/quiz_results", async (HttpContext context, QuizDbContext dbContext) =>
{
    try
    {
        // Get sender (UserId) from query string
            if (!context.Request.Query.TryGetValue("sender", out var senderValue) || !int.TryParse(senderValue, out var userId))
        {
            return Results.Json(new { error = "Invalid or missing sender ID." }, statusCode: 400);
        }

        // Validate UserId exists
        if (!await dbContext.Users.AnyAsync(u => u.Id == userId))
        {
            return Results.Json(new { error = $"User with Id {userId} not found." }, statusCode: 404);
        }

        // Calculate score (count of correct answers)
        User score = await dbContext.Users
            .Where(qr => qr.Id == userId)
            .FirstAsync();

        // Return score (integer if results exist, null if none)

        logger.LogInformation($"Fetched score for UserId={userId}. Score: {score.Score ?? null}");
        return Results.Json(score.Score);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error fetching quiz score");
        return Results.Json(new { error = $"Server error: {ex.Message}" }, statusCode: 500);
    }
});

// New endpoint to get up to 15 random questions
app.MapGet("/get_questions", async (HttpContext context, QuizDbContext dbContext) =>
{
    try
    {
        // Get query parameters
        string sender = context.Request.Query["sender"];
        string content = context.Request.Query["content"];

        // Validate sender and content
        if (string.IsNullOrEmpty(sender) || string.IsNullOrEmpty(content) || !content.StartsWith("start", StringComparison.OrdinalIgnoreCase))
        {
            return Results.Json(new { error = "Missing or invalid sender or content. Content must start with 'start'." }, statusCode: 400);
        }

        // Retrieve up to 15 random questions from the database using SQLite RANDOM()
        var randomQuestions = await dbContext.Questions
            .OrderBy(q => EF.Functions.Random()) // Use SQLite RANDOM() function
            .Take(15)
            .Select(q => new
            {
                q.Id,
                q.QuestionText,
                q.Option1,
                q.Option2,
                q.Option3,
                q.Option4,
                q.img
            })
            .ToListAsync();

        // Log the request
        logger.LogInformation($"Retrieved {randomQuestions.Count} random questions for sender={sender}");

        // Check if fewer than 15 questions were retrieved
        if (randomQuestions.Count < 15)
        {
            logger.LogWarning($"Only {randomQuestions.Count} questions available, less than the requested 15 for sender={sender}");
            return Results.Json(new
            {
                questions = randomQuestions,
                message = $"Only {randomQuestions.Count} questions available in the database."
            });
        }

        // Return the questions as the response
        return Results.Json(randomQuestions);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error retrieving questions");
        return Results.Json(new { error = $"Internal server error: {ex.Message}" }, statusCode: 500);
    }
});
app.MapPost("/submit_results", async (HttpContext context, QuizDbContext dbContext) =>
{
    try
    {
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var clientResults = JsonSerializer.Deserialize<ClientQuestionResult[]>(requestBody, options);

        if (clientResults == null || !clientResults.Any())
        {
            return Results.Json(new { error = "No results provided." }, statusCode: 400);
        }
        int nombre = 0;
        var quizResults = new List<QuizResult>();

        foreach (var clientResult in clientResults)
        {
            // Validate UserId and QuestionId
            if (!await dbContext.Users.AnyAsync(u => u.Id == clientResult.UserId))
            {
                return Results.Json(new { error = $"Invalid UserId: {clientResult.UserId}" }, statusCode: 400);
            }

            var question = await dbContext.Questions.FirstOrDefaultAsync(q => q.Id == clientResult.QuestionId);
            if (question == null)
            {
                return Results.Json(new { error = $"Invalid QuestionId: {clientResult.QuestionId}" }, statusCode: 400);
            }

            // Determine if the answer is correct
            bool isCorrect = string.Equals(clientResult.SelectedOption, question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            // Create QuizResult
            quizResults.Add(new QuizResult
            {
                UserId = clientResult.UserId,
                QuestionId = clientResult.QuestionId,
                SelectedOption = clientResult.SelectedOption,
                IsCorrect = isCorrect
            });
            if (isCorrect)
            {
                nombre++;
            }
        }
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == clientResults[0].UserId);
        user.Score = (nombre * 100 / quizResults.Count).ToString();
        user.CompletedQuiz = DateTime.Now;
        dbContext.Users.Update(user);

        // Save results to database
        dbContext.QuizResults.AddRange(quizResults);
        await dbContext.SaveChangesAsync();

        // Log success
        logger.LogInformation($"Saved {quizResults.Count} quiz results for UserId={clientResults.First().UserId}");
        return Results.Json(new { message = "Results saved successfully" });
    }
    catch (JsonException ex)
    {
        logger.LogError(ex, "Invalid JSON in submit_results request");
        return Results.Json(new { error = "Invalid JSON format." }, statusCode: 400);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing quiz results");
        return Results.Json(new { error = $"Server error: {ex.Message}" }, statusCode: 500);
    }
});
app.Run("http://0.0.0.0:5000");


// Add at the bottom with other models
public class ClientQuestionResult
{
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public string? SelectedOption { get; set; }
}

// Models
public class AuthResult
{
    public bool Success { get; set; }
    public int UserId { get; set; }
}

public record Message
{
    [JsonPropertyName("sender")]
    public string Sender { get; init; }
    [JsonPropertyName("content")]
    public string Content { get; init; }
}