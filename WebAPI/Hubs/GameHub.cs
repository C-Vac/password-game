using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace PasswordGameWebAPI.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
