using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PasswordGameWebAPI.Hubs;
using PasswordGameWebAPI.Models;
using PasswordGameWebAPI.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace PasswordGameWebAPI.Controllers;
[ApiController]
[Route("api/game")]
public class GameController : ControllerBase
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly Dictionary<string, GameRoom> _gameRooms = new Dictionary<string, GameRoom>();

    // Constructor

    public GameController(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // Requests

    [HttpGet("gameState/{roomId}")]
    public IActionResult GetGameState(string roomId)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // Create a DTO (Data Transfer Object) to represent the game state
            var gameState = new GameStateDto
            {
                Teams = gameRoom.Teams.Select(t => new TeamDto
                {
                    TeamId = t.TeamId,
                    Players = t.Players.Select(p => new PlayerDto
                    {
                        PlayerName = p.Name
                    }).ToList(),
                    Score = t.Score
                }).ToList(),
                CurrentRound = gameRoom.CurrentRound,
                CurrentClue = gameRoom.CurrentClue,
                State = gameRoom.State
            };

            return Ok(gameState);
        }
        else
        {
            return NotFound("Room not found.");
        }
    }

    [HttpPost("changeTeam/{roomId}")]
    public IActionResult ChangeTeam(string roomId, [FromBody] ChangeTeamRequest request)
    {
        if (_gameRooms.TryGetValue(roomId, out var gameRoom))
        {
            // Find the player
            Player? player = gameRoom.Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.ConnectionId == request.ConnectionId);

            if (player != null)
            {
                // Remove the player from their current team
                Team? currentTeam = gameRoom.Teams.FirstOrDefault(t => t.Players.Contains(player));
                if (currentTeam != null)
                {
                    currentTeam.RemovePlayer(player);
                }

                // Add the player to the new team
                Team? newTeam = gameRoom.Teams.FirstOrDefault(t => t.TeamId == request.NewTeamId);
                if (newTeam != null)
                {
                    newTeam.AddPlayer(player);

                    // Notify all clients in the room about the team change
                    _hubContext.Clients.Group(roomId).SendAsync("PlayerChangedTeam", player.Name, request.NewTeamId);

                    return Ok();
                }
            }

            return BadRequest("Invalid player or team.");
        }
        else
        {
            return NotFound("Room not found.");
        }
    }

    [HttpPost("changeName/{roomId}")]
public IActionResult ChangeName(string roomId, [FromBody] ChangeNameRequest request)
{
    if (_gameRooms.TryGetValue(roomId, out var gameRoom))
    {
        // Find the player
        Player? player = gameRoom.Teams.SelectMany(t => t.Players).FirstOrDefault(p => p.ConnectionId == request.ConnectionId);

        if (player != null)
        {
            // Update the player's name
            player.Name = request.NewName;

            // Notify all clients in the room about the name change
            _hubContext.Clients.Group(roomId).SendAsync("PlayerChangedName", player.Name);

            return Ok();
        }

        return BadRequest("Invalid player.");
    }
    else
    {
        return NotFound("Room not found.");
    }
}
}
