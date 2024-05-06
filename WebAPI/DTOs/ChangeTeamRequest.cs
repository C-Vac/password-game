namespace PasswordGameWebAPI.Dtos;

public class ChangeTeamRequest
{
    public string ConnectionId { get; set; }
    public string NewTeamId { get; set; }
}
