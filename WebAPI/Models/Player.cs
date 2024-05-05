namespace PasswordGameWebAPI.Models
{
    public class Player
    {
        public string PlayerId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string ConnectionId { get; set; } // To identify the connected client
        public bool IsClueGiver { get; set; }

        public Player(string name, string connectionId)
        {
            Name = name;
            ConnectionId = connectionId;
        }
    }
}
