

using System.Threading.Tasks;
using demoProject;
using demoProject.Models;

namespace TriviaGame{

    public class TriviaGameRunner{

        private TriviaQuestion _CurrentQuestion {get; set;} = new();
        private int _Score {get; set;}
        private string _User {get; set;}
        private bool _ContinueGame {get; set;} = true;

        public TriviaGameRunner(string user){
            this._Score = 0;
            this._User = user;
        }


        public async Task RunGame (){

            var question = await GetQuestion();
            SetCurrentQuestion(question);
            while(this._ContinueGame){

                AskQuestion();
                ShowScore();
                AskToContinue();
                var nextQuestion = await GetQuestion();
                SetCurrentQuestion(nextQuestion);
            }

        }
        private void AskQuestion(){
            Console.WriteLine("\n=== TRIVIA QUESTION ===");
            if (!string.IsNullOrEmpty(this._CurrentQuestion.category))
            Console.WriteLine($"Category: {this._CurrentQuestion.category}");
            if (!string.IsNullOrEmpty(this._CurrentQuestion.difficulty))
            Console.WriteLine($"Difficulty: {this._CurrentQuestion.difficulty.ToUpper()}");

            Console.WriteLine($"\nQ: {this._CurrentQuestion.question ?? "No question available"}");
            Console.Write("\nYour answer: ");
            string answer = Console.ReadLine() ?? "";

            CheckIfCorrect(answer);

            if (this._CurrentQuestion.incorrect_answers?.Count > 0)
            {
            Console.WriteLine("\nOther possible answers were:");
                foreach (var incorrect in this._CurrentQuestion.incorrect_answers)
                {
                    Console.WriteLine($"- {incorrect}");
                }
            }
            Console.WriteLine("=====================");
        }

        private async Task<TriviaQuestion> GetQuestion (){
            TriviaQuestion question = await QuestionGenerator.GetTriviaQuestion();
            return question;
        }             

        public void CheckIfCorrect(string userAnswer){
            if(userAnswer.ToLower() == _CurrentQuestion.correct_answer.ToLower()) SetScore(_Score++);
            else if(_CurrentQuestion.correct_answer.ToLower().Contains(userAnswer.ToLower())){
                Console.WriteLine("That was close! I will give it to you! + 1");
                SetScore(_Score++);
            }
            else Console.WriteLine($"That is incorrect - the correct answer was {_CurrentQuestion.correct_answer}");
        }

        public void ShowScore(){
            Console.WriteLine($"{this._User}'s score: {this._Score}");
        }

        public void AskToContinue(){
            Console.WriteLine($"{this._User} - do you want to continue? (y/n)");
            bool continueTrvia = Console.ReadLine()?.ToLower().StartsWith("y") ?? false;
            SetContinueGame(continueTrvia);
        }

        private void SetCurrentQuestion(TriviaQuestion triviaQuestion){
            this._CurrentQuestion = triviaQuestion;
        }

        private void SetScore(int score){
            this._Score = score;
        }

        public int GetScore(){
            return this._Score;
        }

        private void SetContinueGame(bool wantsToContinue){
            this._ContinueGame = wantsToContinue;
        }
    }
}

