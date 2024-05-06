namespace PasswordGameWebAPI.Dtos;

public class TeamDto
{
    public string TeamId { get; set; }
    public List<PlayerDto> Players { get; set; }
    public int Score { get; set; }
}
