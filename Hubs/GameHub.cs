using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TriviaR.Hubs
{
    public class GameHub : Hub
    {
        static int CurrentUserCount { get; set; }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async void PushQuestion()
        {
            await Clients.All.SendAsync("receiveQuestion");
        }
    }
}