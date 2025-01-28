using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using demoProject.Models;

namespace demoProject
{
    /// <summary>
    /// Generates trivia and casual questions using the Open Trivia Database API.
    /// </summary>
    public class QuestionGenerator
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string apiBaseUrl = "https://opentdb.com/api.php";
        private static readonly List<string> casualQuestions = new List<string>
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

        /// <summary>
        /// Fetches a random trivia question from the Open Trivia Database.
        /// </summary>
        /// <param name="difficulty">Optional difficulty level (easy, medium, hard)</param>
        /// <param name="category">Optional category ID (see OpenTDB documentation)</param>
        /// <returns>A trivia question or null if the API request fails</returns>
        public static async Task<TriviaQuestion?> GetTriviaQuestion(string? difficulty = null, int category = 0)
        {
            try
            {
                // Build API URL with parameters
                var queryParams = BuildQueryParameters(difficulty, category);
                var url = $"{apiBaseUrl}?{string.Join("&", queryParams)}";
                
                // Fetch and parse response
                var response = await httpClient.GetStringAsync(url);
                var question = ParseApiResponse(response);
                
                if (question != null)
                {
                    DecodeHtmlEntities(question);
                    return question;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching trivia: {ex.Message}\nStack trace: {ex.StackTrace}");
            }

            return null;
        }

        /// <summary>
        /// Gets a random casual question for user interaction.
        /// </summary>
        /// <returns>A random question from the casual questions list.</returns>
        public static string GetCasualQuestion()
        {
            int index = random.Next(casualQuestions.Count);
            return casualQuestions[index];
        }

        private static List<string> BuildQueryParameters(string? difficulty, int category)
        {
            var parameters = new List<string> { "amount=1" };
            
            if (!string.IsNullOrEmpty(difficulty))
                parameters.Add($"difficulty={difficulty.ToLower()}");
            
            if (category > 0)
                parameters.Add($"category={category}");

            return parameters;
        }

        private static TriviaQuestion? ParseApiResponse(string response)
        {
            try
            {
                var json = JToken.Parse(response);
                var firstResult = json["results"]?[0];
                
                if (firstResult == null)
                    return null;

                return new TriviaQuestion
                {
                    Category = firstResult["category"]?.ToString(),
                    Type = firstResult["type"]?.ToString(),
                    Difficulty = firstResult["difficulty"]?.ToString(),
                    Question = firstResult["question"]?.ToString(),
                    CorrectAnswer = firstResult["correct_answer"]?.ToString(),
                    IncorrectAnswers = firstResult["incorrect_answers"]?
                        .ToObject<List<string>>()
                        ?.Where(a => a != null)
                        .Select(a => a!)
                        .ToList() ?? new List<string>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing API response: {ex.Message}");
                return null;
            }
        }

        private static void DecodeHtmlEntities(TriviaQuestion question)
        {
            question.Question = WebUtility.HtmlDecode(question.Question);
            question.CorrectAnswer = WebUtility.HtmlDecode(question.CorrectAnswer);
            
            if (question.IncorrectAnswers != null)
            {
                question.IncorrectAnswers = question.IncorrectAnswers
                    .Select(WebUtility.HtmlDecode)
                    .ToList();
            }
        }
    }
}
