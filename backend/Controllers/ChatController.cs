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

        [HttpPost("create_new_chat")]
        public async Task<IActionResult> CreateNewChat(string chatName) =>
            await _chatService.CreateNewChat(User, chatName);

        [HttpPost("update_chat_name")]
        public async Task<IActionResult> UpdateChatName(string chatId, string newName) =>
            await _chatService.UpdateChatName(User, chatId, newName);

        [HttpPost("add_new_members")]
        public async Task<IActionResult> AddNewMembers(string chatId, List<string> friendsId) =>
            await _chatService.AddNewMembers(User, chatId, friendsId);

        [HttpPost("send_message")]
        public async Task<IActionResult> SendMessage(string chatId, string textMessage) =>
            await _chatService.SendMessage(User, chatId, textMessage);

        [HttpGet("get_message_chunk")]
        public async Task<IActionResult> GetMessageChunk(string chatId, int chunkNumber) =>
            await _chatService.GetMessageChunk(User, chatId, chunkNumber);
    }
}
