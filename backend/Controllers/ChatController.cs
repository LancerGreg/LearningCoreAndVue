using backend.Managers.ActionResult;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("get_chats_by_current_user")]
        public async Task<JsonResult> GetChatsByCurrentUser() =>
            new JsonResult(await _chatService.GetChatsByCurrentUser(User));

        [HttpGet("get_message_chunk")]
        public async Task<IActionResult> GetMessageChunk(string chatId) =>
            await _chatService.GetMessages(User, chatId);

        [HttpGet("get_users_by_name")]
        public async Task<IActionResult> GetUsersByName(string chatId, string fullName) =>
            await _chatService.GetUsersByName(User, chatId, fullName ?? "");

        [HttpPost("create_new_chat")]
        public async Task<IActionResult> CreateNewChat(string chatName) =>
            await _chatService.CreateNewChat(User, chatName);

        [HttpPost("update_chat_name")]
        public async Task<IActionResult> UpdateChatName(string chatId, string newName) =>
            await _chatService.UpdateChatName(User, chatId, newName);

        [HttpPost("add_new_member")]
        public async Task<IActionResult> AddNewMembers(string chatId, string memberId) =>
            await _chatService.AddNewMembers(User, chatId, memberId);

        [HttpPost("send_message")]
        public async Task<IActionResult> SendMessage(string chatId, string textMessage) =>
            await _chatService.SendMessage(User, chatId, textMessage);

        [HttpDelete("leave_chat")]
        public async Task<IActionResult> LeaveChat(string chatId) =>
            await _chatService.LeaveChat(User, chatId);
    }
}
