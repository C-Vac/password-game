namespace Games.Password;

public class Lobby
{
    public readonly Guid LobbyID = Guid.NewGuid(); // Unique ID for each lobby
    public GameState GState { get; private set; } = GameState.Lobby; // Track game state with enum
    public List<Player> Players { get; } = new(); // Store players in a list
    public Player HostPlayer { get; private set; }
    public PasswordGame? Game { get; private set; }

    public Lobby(Player host)
    {
        Console.WriteLine($"New lobby created, ID: {LobbyID}");
        HostPlayer = host;
        Players.Add(host);
    }
    public void StartGame()
    {
        Console.WriteLine("Starting new game...");

        while (Players.Count < 4)
        {
            Console.WriteLine("Lobby is not full, adding AI players.");
            AddAIPlayers();
        }

        GState = GameState.InProgress;

        // Create teams from the players list
        Team team1 = new Team(Players[0], Players[1]);
        Team team2 = new Team(Players[2], Players[3]);

        Game = new PasswordGame(team1, team2); // Pass the teams to the game
    }

    public void NextRound()
    {

        Console.WriteLine("Starting new round...");
        Game?.NewRound();
    }

    public void EndGame()
    {
        Console.WriteLine("Game over.");
        GState = GameState.GameOver;
        Game = null;
    }

    private int GetTeamPoints(Team team)
    {
        return team.Points;
    }

    public void AddPlayer(string playerName)
    {
        if (GState != GameState.Lobby)
        {
            Console.WriteLine("Game already started, cannot add new players.");
            return;
        }

        var newPlayer = new Player(playerName);
        Players.Add(newPlayer);
        Console.WriteLine($"{newPlayer.Name} joined the lobby.");

        if (Players.Count == 4)
        {
            Console.WriteLine("Lobby is full, starting game automatically!");
            StartGame();
        }
    }

    private void AddAIPlayers()
    {
        string[] aiNames = { "🤖 Bot1", "🤖 Bot2", "🤖 Bot3" };

        foreach (string name in aiNames)
        {
            if (Players.Count >= 4) break;
            AddPlayer(name);
        }
    }
}

public enum GameState
{
    Lobby,
    InProgress,
    GameOver
}
