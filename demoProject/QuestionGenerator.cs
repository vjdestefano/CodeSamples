using System;
using System.Collections.Generic;

namespace demoProject
{
    public class QuestionGenerator
    {
        private static readonly List<string> questions = new List<string>
        {
            "What's your favorite color?",
            "If you could travel anywhere right now, where would you go?",
            "What's your favorite food?",
            "What's your dream job?",
            "If you could have any superpower, what would it be?",
            "What's your favorite season of the year?",
            "If you could learn any skill instantly, what would it be?",
            "What's your favorite hobby?"
        };

        private static readonly Random random = new Random();

        public static string GetRandomQuestion()
        {
            int index = random.Next(questions.Count);
            return questions[index];
        }
    }
}
