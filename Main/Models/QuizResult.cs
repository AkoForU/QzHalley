using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class QuizResult
    {
        public int Id { get; set; } // Primary key for the result
        public User? User { get; set; }
        public int UserId { get; set; } // Foreign key to User.Id
        public Question? Question { get; set; }
        public int QuestionId { get; set; } // Foreign key to Question.Id
        public string? SelectedOption { get; set; } // The option the user chose
        public bool IsCorrect { get; set; } // Whether the selection was correct
    }
}
