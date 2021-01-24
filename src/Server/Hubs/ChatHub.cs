using System;
using System.Threading.Tasks;
using Beedux.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Beedux.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        [HubMethodName("send")]
        public async Task SendMessage(string message)
        {
            var userId = Context.UserIdentifier;

            await Clients.All.SendAsync("receive", new ChatMessage
            {
                User = userId!,
                Timestamp = DateTimeOffset.UtcNow,
                Content = message
            });
        }
    }
}
