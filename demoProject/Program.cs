using System;
using System.Threading.Tasks;
using demoProject.Models;
using TriviaGame;

namespace demoProject
{
    /// <summary>
    /// Main program class that handles the interactive trivia game experience.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point of the application. Manages user interaction and game flow.
        /// </summary>
        /// <param name="args">Command line arguments - expects the user's name</param>
        static async Task Main(string[] args)
        {
            // Validate command line arguments for proper usage
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- <your name>");
                Console.WriteLine("Example: dotnet run -- John");
                return;
            }

            string name = args[0];
            Console.WriteLine($"Hello, {name}!");
            Console.WriteLine($"Welcome to our demo console application.");
            
            // Display current time to provide context
            Console.WriteLine($"\nCurrent time: {DateTime.Now}");
            
            // Generate dynamic greeting based on the time of day using pattern matching
            int hour = DateTime.Now.Hour;
            string timeMessage = hour switch
            {
                >= 5 and < 12 => "Have a great morning",
                >= 12 and < 17 => "Have a wonderful afternoon",
                >= 17 and < 22 => "Have a pleasant evening",
                _ => "Have a good night"
            };
            
            Console.WriteLine($"{timeMessage}, {name}!");

            // Let user choose between trivia and casual questions
            Console.WriteLine($"\nHey {name}, let me ask you something!");
            Console.Write("Would you like a trivia question? (y/n): ");
            bool useTrivia = Console.ReadLine()?.ToLower().StartsWith("y") ?? false;

            // Handle game flow based on user's choice
            if (useTrivia)
            {
                // Start trivia game session
                TriviaGameRunner Game = new TriviaGameRunner(name);
                await Game.RunGame();
            }
            else
            {
                // Present a casual conversation question
                string randomQuestion = QuestionGenerator.GetCasualQuestion();
                Console.WriteLine(randomQuestion);
                Console.Write("Your answer: ");
                string answer = Console.ReadLine() ?? "";
            }
            
            Console.WriteLine($"\nThanks for playing, {name}!");
        }
    }
}
