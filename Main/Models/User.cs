using System.ComponentModel.DataAnnotations;
namespace Main.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Score { get; set; } //if is null the string will have the value "null" or a number
        public DateTime? Created { get; set; }
        public DateTime? CompletedQuiz { get; set;}
    }
}
