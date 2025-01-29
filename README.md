# Interactive Console Trivia Game

A C# console application that provides an interactive experience with trivia questions from the Open Trivia Database API and casual conversation questions.

## Features

- 🎮 Interactive command-line interface
- 🌍 Integration with Open Trivia Database API
- 🎯 Dynamic question generation
- ⏰ Time-based personalized greetings
- 🎲 Choice between trivia and casual questions
- 🔄 Error handling and graceful fallbacks

## Project Structure

```
demoProject/
├── Models/
│   ├── TriviaQuestion.cs    # Data model for trivia questions
│   └── TriviaResponse.cs    # API response model
├── Program.cs               # Main application entry point
└── QuestionGenerator.cs     # Question generation and API integration
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Internet connection (for trivia questions)

### Running the Application

1. Clone the repository
2. Navigate to the project directory:
   ```bash
   cd demoProject
   ```
3. Run the application with your name:
   ```bash
   dotnet run -- <your name>
   ```
   Example:
   ```bash
   dotnet run -- Alice
   ```

## How It Works

1. **Initialization**
   - The application takes a user's name as a command-line argument
   - Displays a personalized greeting based on the current time of day

2. **Question Selection**
   - Users can choose between trivia questions or casual conversation questions
   - Trivia questions are fetched from the Open Trivia Database API
   - Casual questions are randomly selected from a predefined list

3. **Interactive Experience**
   - For trivia questions:
     - Displays category and difficulty
     - Shows the question and waits for user input
     - Reveals the correct answer and other possible answers
   - For casual questions:
     - Presents a random conversation starter
     - Accepts and acknowledges user input

## Technical Details

### API Integration
- Uses the Open Trivia Database API (opentdb.com)
- Handles HTML entity decoding for API responses
- Implements error handling for API requests

### Key Components

- **QuestionGenerator**: Manages question generation and API integration
  - Fetches trivia questions from the API
  - Provides fallback casual questions
  - Handles response parsing and HTML decoding

- **Models**:
  - `TriviaQuestion`: Represents individual trivia questions with properties for category, difficulty, question text, and answers
  - `TriviaResponse`: Encapsulates the API response structure

## Error Handling

The application includes robust error handling:
- Graceful fallback to casual questions if API requests fail
- Input validation for command-line arguments
- HTML entity decoding for clean question display
- Null-safe string operations

## Contributing

Feel free to submit issues and enhancement requests!

## License

This project is open source and available under the MIT License.
