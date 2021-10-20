using backend.Managers.ActionResult;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<object>> GetChatsByCurrentUser(ClaimsPrincipal curentUser);
        Task<IActionResult> CreateNewChat(ClaimsPrincipal curentUser, List<string> friendsId);
        Task<IActionResult> UpdateChatName(ClaimsPrincipal curentUser, string chatId, string newName);
        Task<IActionResult> AddNewMembers(ClaimsPrincipal curentUser, string chatId, List<string> friendsId);
        Task<IActionResult> SendMessage(ClaimsPrincipal curentUser, string chatId, string textMessage);
        Task<IActionResult> GetMessageChunk(ClaimsPrincipal curentUser, string chatId, int chunkNumber);
    }
}
