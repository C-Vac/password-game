using System;
using System.Collections.Generic;

namespace Games.Password;


/// <summary>
/// An instance of the Password game.
/// </summary>
public class PasswordGame
{
    private int currentRound = 1;
    private string currentPassword = "";
    private (Team, Team) teams;
    private Team currentTeam; // the team that gets to guess
    private int PointsToWin = 3;

    public List<string> clues { get; private set; } = new(); // the list of clues for the current round

    public PasswordGame(Team team1, Team team2)
    {
        teams = (team1, team2);
        currentTeam = team1;
    }

    public void RunGameLoop()
    {
        string? clue = "";
        string? guess = "";

        while (true) // Loop until the game ends
        {

            NewRound();

            // Get clue from clue giver
            clue = GetClue();

            // Get guess from guesser
            guess = GetGuess();

            // Switch teams
            currentTeam = (currentTeam == teams.Item1) ? teams.Item2 : teams.Item1;

            // Display current clues
            Console.WriteLine("Clues given so far:");
            foreach (string c in clues)
            {
                Console.WriteLine($"- {c}");
            }
        }

        string GetClue()
        {
            Console.Write($"{currentTeam.player1.Name}, enter your clue: ");
            while (string.IsNullOrEmpty(clue))
            {
                clue = Console.ReadLine();
            }
            GiveClue(clue);
            return clue;
        }

        string GetGuess()
        {
            Console.Write($"{currentTeam.player2.Name}, enter your guess: ");
            while (string.IsNullOrEmpty(guess))
            {
                guess = Console.ReadLine();
            }
            MakeGuess(guess);
            return guess;
        }
    }


    public void NewRound()
    {
        clues = new List<string>();
        currentPassword = GenerateRandomPassword();

        // Display the password only to the clue giver
        Console.WriteLine($"{currentTeam.player1.Name}, the password is: {currentPassword}");
    }

    public void GiveClue(string clue)
    {
        Console.WriteLine($"{currentTeam.player1.Name} gives a clue: {clue}");
        clues.Add(clue);
    }

    public List<string> GetClues()
    {
        return clues;
    }

    public void MakeGuess(string guess)
    {
        Console.WriteLine($"{currentTeam.player2.Name} guesses: {guess}");
        if (guess.ToLower() == currentPassword.ToLower()) // Case-insensitive comparison
        {
            Console.WriteLine("Correct guess!");
            currentTeam.AddPoints(1);
            CheckWinCondition(); // See if the team reached the points to win
        }
        else
        {
            Console.WriteLine("Incorrect guess, next team's turn.");
            // ... logic to switch to the other team ...
        }
    }

    private void CheckWinCondition()
    {
        if (currentTeam.Points >= PointsToWin)
        {
            Console.WriteLine($"{currentTeam.player1.Name} and {currentTeam.player2.Name} win the game!");
            // ... logic to end the game ...
        }
        else
        {
            NewRound(); // Start a new round if nobody won yet
        }
    }

    private string GenerateRandomPassword()
    {
        string[] words = {
            "table", "chair", "phone", "car", "book", "pen", "shoe", "lamp", "door", "window", "bed", "computer",
            "television", "refrigerator", "stove", "oven", "microwave", "toaster", "blender", "coffee maker",
            "dog", "cat", "bird", "fish", "snake", "lion", "tiger", "bear", "elephant", "monkey", "horse",
            "cow", "pig", "sheep", "goat", "chicken", "duck", "rabbit", "turtle", "frog", "pizza", "burger",
            "pasta", "salad", "soup", "sandwich", "taco", "burrito",
            "sushi", "ice cream", "cake", "cookie", "pie", "fruit", "vegetable", "bread", "cheese", "milk",
            "juice", "water", "home", "school", "work", "park", "beach", "store", "restaurant", "hospital",
            "library", "museum", "airport", "train station", "bus stop", "church", "temple", "mosque",
            "love", "hate", "peace", "war", "time", "space", "energy", "matter", "gravity", "light", "sound", "heat", "cold"
        };

        Random random = new Random();
        int wordIndex = random.Next(words.Length);
        return words[wordIndex];
    }
}
