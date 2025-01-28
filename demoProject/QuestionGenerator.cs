using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using demoProject.Models;
using Newtonsoft.Json;

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
                List<string> queryParams = BuildQueryParameters(difficulty, category);
                string url = $"{apiBaseUrl}?{string.Join("&", queryParams)}";
                
                // Fetch and parse response
                string response = await httpClient.GetStringAsync(url);
                TriviaQuestion? question = ParseApiResponse(response);
                
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
            Random random = new Random();
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
                TriviaResponse json = JsonConvert.DeserializeObject<TriviaResponse>(response) ?? new ();
                TriviaQuestion firstResult = json.Results[0];
                
                if (firstResult == null) return null;
                else return firstResult;
                    

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing API response: {ex.Message}");
                return null;
            }
        }

        private static void DecodeHtmlEntities(TriviaQuestion question)
        {
            question.question = WebUtility.HtmlDecode(question.question);
            question.correct_answer = WebUtility.HtmlDecode(question.correct_answer);
            
            if (question.incorrect_answers != null)
            {
                question.incorrect_answers = question.incorrect_answers
                    .Select(WebUtility.HtmlDecode)
                    .ToList();
            }
        }
    }
}
