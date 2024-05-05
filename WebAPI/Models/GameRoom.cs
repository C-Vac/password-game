using System;
using System.Collections.Generic;
using PasswordGameWebAPI.Hubs;
using System.Timers;

namespace PasswordGameWebAPI.Models;

public enum GameState
{
    Lobby,
    Started,
    ClueGiving,
    Guessing,
    GameOver
}

public class GameRoom
{
    public string RoomId { get; }
    public List<Team> Teams { get; } = new List<Team>();
    public GameState State { get; private set; } = GameState.Lobby;
    public string CurrentClue { get; private set; }
    public int CurrentRound { get; private set; } = 1;
    private List<string> _passwords;
    private string _currentPassword;

    // Timers
    private System.Timers.Timer _clueTimer;
    private System.Timers.Timer _guessTimer;

    public GameRoom(string roomId)
    {
        RoomId = roomId;
    }
    public void StartCluePhase()
    {
        // Start the clue timer
        _clueTimer = new System.Timers.Timer(5000); // 5 seconds
        _clueTimer.Elapsed += OnClueTimerElapsed;
        _clueTimer.Start();
    }

    private void OnClueTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Handle clue timer timeout
        // ...
    }

    // ... similar implementation for guess timer ...

    public string CheckIfRoundWon()
    {
        // Check if any team has guessed the password correctly
        foreach (Team team in Teams)
        {
            if (team.Players.Any(p => p.LastGuess != null && p.LastGuess.Equals(_currentPassword, StringComparison.OrdinalIgnoreCase)))
            {
                return team.TeamId;
            }
        }

        return null; // No team won the round
    }
    public void GeneratePassword()
    {
        if (_passwords == null || _passwords.Count == 0)
        {
            // Load passwords from a file or API
            _passwords = LoadPasswordsFromFile("passwords.txt"); // Example
        }

        // Choose a random password from the list
        Random random = new Random();
        int index = random.Next(_passwords.Count);
        _currentPassword = _passwords[index];
        _passwords.RemoveAt(index); // Remove the used password
    }
    public bool IsValidClue(string clue)
    {
        // Check if the clue is a substring of the password (case-insensitive)
        return !_currentPassword.Contains(clue, StringComparison.OrdinalIgnoreCase);
    }
    public bool IsGameOver()
    {
        return false;
        // TODO: AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
    }

    public string CheckGuess(string guess)
    {
        if (guess.Equals(_currentPassword, StringComparison.OrdinalIgnoreCase))
        { // Find the player who made the guess 
            Player guessingPlayer = Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.ConnectionId == null); // TODO: MAKE THIS WORK --> Context.ConnectionId); 

            if (guessingPlayer != null)
            {
                // Find the team of the guessing player
                Team winningTeam = Teams.FirstOrDefault(t => t.Players.Contains(guessingPlayer));
                if (winningTeam != null)
                {
                    winningTeam.AddPoints(1);
                    return winningTeam.TeamId;
                }
            }
        }

        return null;
    }
    public void StartNewRound()
    {
        CurrentRound++;
        GeneratePassword();
        CurrentClue = null;
        // Switch clue giver and guesser roles within each team
        foreach (Team team in Teams)
        {
            team.SwitchRoles();
        }
        State = GameState.ClueGiving;
    }
    public string AddPlayerToTeam(string playerName)
    {
        // Find a team with less than 2 players
        Team team = Teams.FirstOrDefault(t => t.Players.Count < 2);

        // If no such team exists, create a new one
        if (team == null)
        {
            team = new Team();
            Teams.Add(team);
        }

        // Add the player to the team
        team.AddPlayer(new Player(playerName, "")); // TODO: inject connectionId

        return team.TeamId;
    }

    public bool CanStartGame()
    {
        // Check if there are at least two teams with two players each
        return Teams.Count >= 2 && Teams.All(t => t.Players.Count == 2);
    }

    public void StartGame()
    {
        State = GameState.Started;
        // ... additional initialization logic ...
    }

    public void StartGuessPhase()
    {
        State = GameState.Guessing;
    }

    public void SetClue(string clue)
    {
        CurrentClue = clue;
    }

    // ... other methods for managing rounds, turns, scores, etc. ...
    private List<string> LoadPasswordsFromFile(string filePath)
    {
        // Implement file reading logic here
        // ...
        return new List<string>(); // Replace with actual password list
    }
}
