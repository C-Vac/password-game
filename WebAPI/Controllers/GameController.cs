using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PasswordGameWebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace PasswordGameWebAPI.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IHubContext<GameHub> _hubContext;

        public GameController(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("send-message")]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
            return Ok();
        }
    }
}
