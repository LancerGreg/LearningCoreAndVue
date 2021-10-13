using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.SignalR
{
    public class NotifyService
    {
        private readonly IHubContext<ChatHub> _hub;

        public NotifyService(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public Task SendNotificationAsync(string message)
        {
            return _hub.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}