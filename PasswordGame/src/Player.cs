namespace Games.Password;

public class Player
{
    public string Name { get; set; }
    public int Score { get; set; } = 0;
    public string Avatar { get; set; } = "😎"; // Default avatar

    public Player(string name)
    {
        Name = name;
    }
}
