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
}
