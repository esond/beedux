using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Meeteor.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        [HubMethodName("send")]
        public async Task SendMessage(string message)
        {
            var userId = Context.UserIdentifier;

            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
    }
}
