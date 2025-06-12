using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class QuizDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=../../../../db/quizapp.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(100); // Optional: Add length constraint
            modelBuilder.Entity<User>().Property(u => u.Password).HasMaxLength(100); // Optional: Add length constraint
            modelBuilder.Entity<User>().Property(u => u.Score).HasMaxLength(100); // Optional: Limit score length

            // Configure Question entity
            modelBuilder.Entity<Question>().Property(q => q.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Question>().Property(q => q.QuestionText).HasMaxLength(500); // Optional: Add length constraint
            modelBuilder.Entity<Question>().Property(q => q.Option1).HasMaxLength(100);
            modelBuilder.Entity<Question>().Property(q => q.Option2).HasMaxLength(100);
            modelBuilder.Entity<Question>().Property(q => q.Option3).HasMaxLength(100);
            modelBuilder.Entity<Question>().Property(q => q.Option4).HasMaxLength(100);
            modelBuilder.Entity<Question>().Property(q => q.img).HasMaxLength(255); // Optional: Limit image path length
            modelBuilder.Entity<Question>().Property(q => q.CorrectAnswer).HasMaxLength(100);

            // Configure QuizResult entity
            modelBuilder.Entity<QuizResult>().Property(qr => qr.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<QuizResult>().Property(qr => qr.SelectedOption).HasMaxLength(100);

            // Define relationships
            modelBuilder.Entity<QuizResult>()
                .HasOne(qr => qr.User)
                .WithMany() // No navigation property on User side
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizResult>()
                .HasOne(qr => qr.Question)
                .WithMany() // No navigation property on Question side
                .HasForeignKey(qr => qr.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "TestUser", Password = "password123", Score = "0" }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, QuestionText = "What is the capital of France?", Option1 = "Berlin", Option2 = "Paris", Option3 = "Madrid", Option4 = "Rome", CorrectAnswer = "Paris", img = "qzphotos/question_20250609_1330_1.jpg" },
                new Question { Id = 2, QuestionText = "What is the capital of Spain?", Option1 = "Lisbon", Option2 = "Madrid", Option3 = "Barcelona", Option4 = "Seville", CorrectAnswer = "Madrid", img = "qzphotos/question_20250609_1330_2.jpg" }
            );
        }
    }
}
