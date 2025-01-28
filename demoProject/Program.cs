using System;

namespace demoProject
{
    class Program
    {
        static void Main(string[] args)
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
            var hour = DateTime.Now.Hour;
            string timeMessage = hour switch
            {
                >= 5 and < 12 => "Have a great morning",
                >= 12 and < 17 => "Have a wonderful afternoon",
                >= 17 and < 22 => "Have a pleasant evening",
                _ => "Have a good night"
            };
            
            Console.WriteLine($"{timeMessage}, {name}!");

            // Ask a random question
            Console.WriteLine($"\nHey {name}, I have a question for you:");
            string randomQuestion = QuestionGenerator.GetRandomQuestion();
            Console.WriteLine(randomQuestion);
            
            // Get and display the answer
            Console.Write("Your answer: ");
            string answer = Console.ReadLine() ?? "";
            Console.WriteLine($"\nThanks for sharing that, {name}!");
        }
    }
}
