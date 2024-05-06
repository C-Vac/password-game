namespace PasswordGameWebAPI.Dtos;
using PasswordGameWebAPI.Models;
public class GameStateDto
{
    public List<TeamDto> Teams { get; set; }
    public int CurrentRound { get; set; }
    public string? CurrentClue { get; set; }
    public GameState State { get; set; }
}
