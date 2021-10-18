using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.SignalR
{
    public interface IChatHub
    {
        HubCallerContext HubContext();
        Task SendMessageForAll(string newMessage);
        Task SendMessage(Message newMessage, IEnumerable<string> usersId);
    }
}
