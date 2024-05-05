namespace Games.Password;

public class Team
{
    public int Points { get; private set; } = 0;
    public Player player1 { get; set; }
    public Player player2 { get; set; }

    public Team(Player p1, Player p2)
    {
        player1 = p1;
        player2 = p2;
    }

    public void AddPoints(int amount)
    {
        Points += amount;
    }
}
