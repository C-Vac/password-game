using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PasswordGameWebAPI.Models;

namespace PasswordGameWebAPI.Hubs;

public class GameHub : Hub
{
    // Store game rooms in a dictionary
    private static Dictionary<string, GameRoom> _gameRooms = new Dictionary<string, GameRoom>();

    public async Task CreateGameRoom(string playerName)
    {
        // Generate a unique room ID
        string roomId = Guid.NewGuid().ToString();

        // Create a new game room
        var gameRoom = new GameRoom(roomId);

        // Add the creating player to a team
        string teamId = gameRoom.AddPlayerToTeam(playerName);

        // Add the game room to the dictionary
        _gameRooms.Add(roomId, gameRoom);

        // Join the group for the specific room
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Return the room ID and team ID to the client
        await Clients.Caller.SendAsync("GameRoomCreated", roomId, teamId);
    }

    public async Task JoinGameRoom(string roomId, string playerName)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // Add the player to a team
            string teamId = gameRoom.AddPlayerToTeam(playerName);

            // Join the group for the specific room
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            // Notify all clients in the room that a new player has joined
            await Clients.Group(roomId).SendAsync("PlayerJoined", playerName, teamId);

            // Return the team ID to the client
            await Clients.Caller.SendAsync("JoinedGameRoom", teamId);

            // If both teams have two players, start the game
            if (gameRoom.CanStartGame())
            {
                await StartGame(roomId);
            }
        }
        else
        {
            await Clients.Caller.SendAsync("Error", "Game room not found.");
        }
    }

    public async Task StartGame(string roomId)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // Start the game
            gameRoom.StartGame();

            // Notify all clients in the room that the game has started
            await Clients.Group(roomId).SendAsync("GameStarted");

            // Start the first round
            await StartNewRound(roomId);
        }
    }
    public async Task StartNewRound(string roomId)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // End the previous round (if any) and update scores
            if (gameRoom.State == GameState.Guessing)
            {
                // Handle the end of the previous round
                // ...
            }

            // Start a new round
            gameRoom.StartNewRound();

            // Notify all clients in the room about the new round and potentially start a new round or end the game.
            await Clients.Group(roomId).SendAsync("NewRound");
        }
    }
    public async Task EndRound(string roomId, string winningTeamId)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // Update the score for the winning team 
            Team winningTeam = gameRoom.Teams.
            FirstOrDefault(t => t.TeamId == winningTeamId);

            if (winningTeam != null)
            {
                winningTeam.AddPoints(1);
            }

        }
        
        // Check if the game has ended
        if (gameRoom.IsGameOver())
        {
            // Declare the winner and end the game
            await Clients.Group(roomId).SendAsync("GameOver", winningTeamId);
            _gameRooms.Remove(roomId); // Remove the game room
        }
        else
        {
            // Start a new round
            await StartNewRound(roomId);
        }
    }
    public async Task SubmitClue(string roomId, string clue)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            if (gameRoom.IsValidClue(clue))
            {
                // Set the current clue
                gameRoom.SetClue(clue);

                // Notify all clients in the room that a clue has been submitted
                await Clients.Group(roomId).SendAsync("ClueSubmitted", clue);

                // Start the guess phase
                gameRoom.StartGuessPhase();
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Invalid clue.");
            }
        }
    }

    public async Task SubmitGuess(string roomId, string guess)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // Check if the guess is correct
            string winningTeamId = gameRoom.CheckGuess(guess);

            if (winningTeamId != null)
            {
                // Notify all clients in the room of the correct guess and winning team
                await Clients.Group(roomId).SendAsync("GuessCorrect", guess, winningTeamId);

                // End the round
                await EndRound(roomId, winningTeamId);
            }
            else
            {
                // Notify the client that the guess was incorrect
                // await Clients.Caller.SendAsync(???);
            }
        }
    }
}
