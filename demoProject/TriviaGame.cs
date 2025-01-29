

using System.Threading.Tasks;
using demoProject;
using demoProject.Models;

namespace TriviaGame{

    public class TriviaGameRunner{

        private TriviaQuestion? _CurrentQuestion {get; set;}
        private int _Score {get; set;}
        private string _User {get; set;}
        private bool _ContinueGame {get; set;} = true;

        public TriviaGameRunner(string user){
            this._Score = 0;
            this._User = user;
        }

        private async Task<bool> StartGame(){
            var question = await GetQuestion();
            
            if(question != null){
                SetCurrentQuestion(question);
                return true;
            } 
            else return false;
        }

        public async Task RunGame (){

            bool startGame = await StartGame();
            if(!startGame) return;
            while(this._ContinueGame){

                AskQuestion();
                ShowScore();
                await AskToContinue();
                
            }

        }
        private void AskQuestion(){
            if(this._CurrentQuestion == null) throw new Exception("current question did not return from API");
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

        private async Task<TriviaQuestion?> GetQuestion (){
            TriviaQuestion? question = await QuestionGenerator.GetTriviaQuestion();
            return question;
        }             

        public void CheckIfCorrect(string userAnswer){
            if(this._CurrentQuestion == null) throw new Exception("current question did not return from API");
            if(this._CurrentQuestion.correct_answer == null) throw new Exception("Had trouble recieving the correct answer");
            
            if(userAnswer.ToLower() == _CurrentQuestion.correct_answer.ToLower()){
                Console.WriteLine("That was correct! + 1");
                SetScore(++_Score);
            }
            else if(_CurrentQuestion.correct_answer.ToLower().Contains(userAnswer.ToLower())){
                Console.WriteLine("That was close! I will give it to you! + 1");
                Console.WriteLine($"The correct answer was {_CurrentQuestion.correct_answer}");
                SetScore(++_Score);
            }
            else Console.WriteLine($"That is incorrect - the correct answer was {_CurrentQuestion.correct_answer}");
        }

        public void ShowScore(){
            Console.WriteLine($"{this._User}'s score: {this._Score}");
        }

        public async Task AskToContinue(){
            Console.WriteLine($"{this._User} - do you want to continue? (y/n)");
            bool continueTrvia = Console.ReadLine()?.ToLower().StartsWith("y") ?? false;
            SetContinueGame(continueTrvia);
            if(continueTrvia){
                var nextQuestion = await GetQuestion();
                if(nextQuestion != null) SetCurrentQuestion(nextQuestion);
                else SetContinueGame(false);
            }
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

