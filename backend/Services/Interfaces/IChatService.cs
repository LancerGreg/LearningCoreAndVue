using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatData>> GetChatsByCurrentUser(ClaimsPrincipal curentUser);
        Task<IActionResult> CreateNewChat(ClaimsPrincipal curentUser, string chatName);
        Task<IActionResult> UpdateChatName(ClaimsPrincipal curentUser, string chatId, string newName);
        Task<IActionResult> AddNewMembers(ClaimsPrincipal curentUser, string chatId, string memberId);
        Task<IActionResult> SendMessage(ClaimsPrincipal curentUser, string chatId, string textMessage);
        Task<IActionResult> GetMessages(ClaimsPrincipal curentUser, string chatId);
        Task<IActionResult> LeaveChat(ClaimsPrincipal curentUser, string chatId);
        Task<IActionResult> GetUsersByName(ClaimsPrincipal curentUser, string chatId, string fullName);
    }
}
