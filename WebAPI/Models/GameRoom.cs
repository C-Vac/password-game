using System;
using System.Collections.Generic;
using PasswordGameWebAPI.Hubs;
using System.Timers;
using Microsoft.AspNetCore.SignalR;

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
    public string RoomId { get; } = "";
    public List<Team> Teams { get; } = new List<Team>();
    public GameState State { get; private set; } = GameState.Lobby;
    public string? CurrentClue { get; private set; } = "";
    public int CurrentRound { get; private set; } = 1;
    private List<string> _passwords = new();
    private string _currentPassword = "";
    public int PointsToWin = 3;
    private readonly GameHub _hub;

    // Timers

    private System.Timers.Timer? _clueTimer;
    private System.Timers.Timer? _guessTimer;

    // Constructor

    public GameRoom(string roomId, GameHub hub)
    {
        RoomId = roomId;
        _hub = hub;
    }

    // Start Game Logic
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
    public void StartCluePhase()
    {
        State = GameState.ClueGiving;

        _clueTimer = new System.Timers.Timer(5000); // 5 seconds
        _clueTimer.Elapsed += OnClueTimerElapsed;
        _clueTimer.Start();
    }

    public void StartGuessPhase()
    {
        State = GameState.Guessing;
        _guessTimer = new System.Timers.Timer(10000); // 10 seconds (example)
        _guessTimer.Elapsed += OnGuessTimerElapsed;
        _guessTimer.Start();
    }

    // Timer event handlers

    private void OnClueTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _clueTimer?.Stop();
        State = GameState.Guessing;
        StartGuessPhase(); // Start the guessing timer
        _hub.Clients.Group(RoomId).SendAsync("ClueTimeUp"); // Notify clients
    }
    private void OnGuessTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _guessTimer?.Stop();

        // Check if any team guessed the password correctly
        string? winningTeamId = CheckIfRoundWon();

        if (IsGameOver())
        {
            // Game over, declare the winner
            _hub.Clients.Group(RoomId).SendAsync("GameOver", winningTeamId);
        }
        else
        {
            // Start a new round
            StartNewRound();
            _hub.Clients.Group(RoomId).SendAsync("NewRound", CurrentRound, CurrentClue);
        }
    }

    // Game logic

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

    public void SetClue(string clue)
    {
        CurrentClue = clue;
    }

    // Password Generation

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
    private List<string> LoadPasswordsFromFile(string filePath)
    {
        // Implement file reading logic here
        // ...
        return new List<string>(); // Replace with actual password list
    }

    // Boolean checks

    public bool IsValidClue(string clue)
    {
        // Check if the clue is a substring of the password (case-insensitive)
        return !_currentPassword.Contains(clue, StringComparison.OrdinalIgnoreCase);
    }
    public bool IsGameOver()
    {
        return Teams.Any(t => t.Score >= PointsToWin);
    }

    // Password/guess match check

    public string? CheckGuess(string guess, string connectionId)
    {
        if (guess.Equals(_currentPassword, StringComparison.OrdinalIgnoreCase))
        {
            // Find the player who made the guess 
            Player? guessingPlayer = Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.ConnectionId == connectionId);

            if (guessingPlayer != null)
            {
                // Find the team of the guessing player
                Team? winningTeam = Teams.FirstOrDefault(t => t.Players.Contains(guessingPlayer));
                if (winningTeam != null)
                {
                    winningTeam.AddPoints(1); // TODO: add variation in points won per round, etc.
                    return winningTeam.TeamId;
                }
            }
        }

        return null;
    }

    public string? CheckIfRoundWon()
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

    // Lobby helper methods

    public string AddPlayerToTeam(string playerName, string connectionId)
    {
        // Find a team with less than 2 players
        Team? team = Teams.FirstOrDefault(t => t.Players.Count < 2);

        // If no such team exists, create a new one
        if (team == null)
        {
            team = new Team();
            Teams.Add(team);
        }

        // Add the player to the team
        team.AddPlayer(new Player(playerName, connectionId));

        return team.TeamId;
    }
}
