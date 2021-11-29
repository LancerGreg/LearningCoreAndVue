using backend.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Managers.SignalR
{
    public interface IChatHub
    {
        HubCallerContext HubContext();
        Task SendMessage(Message newMessage, IEnumerable<string> usersId);
        Task AddToChat(string userId, ChatData chatData);
        Task RenameChatName(IEnumerable<string> usersId, Chat chat);
    }
}
