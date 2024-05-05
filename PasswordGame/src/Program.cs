namespace Games.Password;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Program started. Welcome to PASSWORD.");

        while (true)
        {
            Console.WriteLine("\nChoose an option:\n1. Start a new game\n2. Quit");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Get player name from console
                    Console.Write("Enter your name: ");
                    string playerName = Console.ReadLine() ?? "Player";

                    // Create lobby and player
                    Player player = new Player(playerName);
                    Lobby lobby = new Lobby(player);

                    // Start the game
                    lobby.StartGame();

                    // Run the game loop
                    lobby?.Game?.RunGameLoop();
                    break;

                case "2":
                    Console.WriteLine("Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
