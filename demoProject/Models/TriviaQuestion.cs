using System.Collections.Generic;

namespace demoProject.Models
{
    public class TriviaQuestion
    {
        public string? Category { get; set; }
        public string? Type { get; set; } 
        public string? Difficulty { get; set; }
        public string? Question { get; set; }
        public string? CorrectAnswer { get; set; }
        public List<string>? IncorrectAnswers { get; set; }
    }
}
