using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using demoProject.Models;

namespace demoProject
{
    public class QuestionGenerator
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string baseUrl = "https://opentdb.com/api.php";
        private static readonly List<string> fallbackQuestions = new List<string>
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

        public static async Task<TriviaQuestion?> GetTriviaQuestion(string? difficulty = null, int category = 0)
        {
            try
            {
                var queryParams = new List<string> { "amount=1" };
                
                if (!string.IsNullOrEmpty(difficulty))
                    queryParams.Add($"difficulty={difficulty.ToLower()}");
                
                if (category > 0)
                    queryParams.Add($"category={category}");

                var url = $"{baseUrl}?{string.Join("&", queryParams)}";
                
                Console.WriteLine($"Fetching trivia from: {url}");
                var response = await httpClient.GetStringAsync(url);
                Console.WriteLine($"Raw response: {response}");
                
                // Parse using JToken for more lenient parsing
                var json = JToken.Parse(response);
                var firstResult = json["results"]?[0];
                
                if (firstResult != null)
                {
                    var question = new TriviaQuestion
                    {
                        Category = firstResult["category"]?.ToString(),
                        Type = firstResult["type"]?.ToString(),
                        Difficulty = firstResult["difficulty"]?.ToString(),
                        Question = firstResult["question"]?.ToString(),
                        CorrectAnswer = firstResult["correct_answer"]?.ToString(),
                        IncorrectAnswers = firstResult["incorrect_answers"]?.ToObject<List<string>>()
                    };
                    // Decode HTML entities in all text fields
                    question.Question = WebUtility.HtmlDecode(question.Question);
                    question.CorrectAnswer = WebUtility.HtmlDecode(question.CorrectAnswer);
                    if (question.IncorrectAnswers != null)
                    {
                        question.IncorrectAnswers = question.IncorrectAnswers
                            .Select(a => WebUtility.HtmlDecode(a))
                            .ToList();
                    }
                    return question;
                }
                
                Console.WriteLine("No trivia questions returned from API");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching trivia: {ex.Message}\nStack trace: {ex.StackTrace}");
            }

            return null;
        }

        public static string GetRandomQuestion()
        {
            int index = random.Next(fallbackQuestions.Count);
            return fallbackQuestions[index];
        }

        public static async Task<string> GetQuestion(bool useTrivia = true)
        {
            if (useTrivia)
            {
                var triviaQuestion = await GetTriviaQuestion();
                if (triviaQuestion?.Question != null)
                    return triviaQuestion.Question;
            }
            
            return GetRandomQuestion();
        }
    }
}
