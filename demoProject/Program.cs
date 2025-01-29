using System;
using System.Threading.Tasks;
using demoProject.Models;
using TriviaGame;

namespace demoProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- <your name>");
                Console.WriteLine("Example: dotnet run -- John");
                return;
            }

            string name = args[0];
            Console.WriteLine($"Hello, {name}!");
            Console.WriteLine($"Welcome to our demo console application.");
            
            // Show the time of execution
            Console.WriteLine($"\nCurrent time: {DateTime.Now}");
            
            // Create a personalized message based on time of day
            int hour = DateTime.Now.Hour;
            string timeMessage = hour switch
            {
                >= 5 and < 12 => "Have a great morning",
                >= 12 and < 17 => "Have a wonderful afternoon",
                >= 17 and < 22 => "Have a pleasant evening",
                _ => "Have a good night"
            };
            
            Console.WriteLine($"{timeMessage}, {name}!");

            // Ask a trivia or random question
            Console.WriteLine($"\nHey {name}, let me ask you something!");
            Console.Write("Would you like a trivia question? (y/n): ");
            bool useTrivia = Console.ReadLine()?.ToLower().StartsWith("y") ?? false;

            if (useTrivia)
            {
                    TriviaGameRunner Game = new TriviaGameRunner(name);
                    await Game.RunGame();
            }
            else
            {
                string randomQuestion = QuestionGenerator.GetCasualQuestion();
                Console.WriteLine(randomQuestion);
                Console.Write("Your answer: ");
                string answer = Console.ReadLine() ?? "";
            }
            
            Console.WriteLine($"\nThanks for playing, {name}!");
        }
    }
}
