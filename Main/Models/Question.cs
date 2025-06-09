using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? img { get; set; }
        public string? CorrectAnswer { get; set; }

        public int OptionsCount
        {
            get
            {
                int count = 0;
                if (!string.IsNullOrEmpty(Option1)) count++;
                if (!string.IsNullOrEmpty(Option2)) count++;
                if (!string.IsNullOrEmpty(Option3)) count++;
                if (!string.IsNullOrEmpty(Option4)) count++;
                return count;
            }
        }
    }
}
