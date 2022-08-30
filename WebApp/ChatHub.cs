using WebApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace WebApp
{
    public class ChatHub: Hub
    {
        public async Task BroadcastAsync(ChatMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
    public interface IChatHub
    {
        Task MessageReceivedFromHub(ChatMessage message);

        Task NewUserConnected(string message);
    }
}
