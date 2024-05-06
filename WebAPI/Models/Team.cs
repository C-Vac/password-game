namespace PasswordGameWebAPI.Models
{
    public class Team
    {
        public string TeamId { get; set; } = Guid.NewGuid().ToString();
        public List<Player> Players { get; } = new List<Player>();
        public int Score { get; private set; } = 0;

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }

        public void AddPoints(int points)
        {
            Score += points;
        }

        public void SwitchRoles()
        {
            // Implement logic to switch clue giver and guesser roles
            // ...
        }
    }
}
