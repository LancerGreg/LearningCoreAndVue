using backend.Managers.ActionResult;
using backend.Managers.ActionResult.Responses;
using backend.Managers.SignalR;
using backend.Models;
using backend.Repositories;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services
{
    public class ChatService : AppDbRepository, IChatService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IChatHub _chatHub;

        public ChatService(AppDbContext dbContext, UserManager<AppUser> userManager, IChatHub chatHub) : base(dbContext)
        {
            _userManager = userManager;
            _chatHub = chatHub;
        }

        public async Task<IEnumerable<Chat>> GetChatsByCurrentUser(ClaimsPrincipal curentUser)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            return dbContext.Chats.Where(_ => dbContext.ChatBridges.Where(cb => cb.UserId == user.Id).Select(cb => cb.ChatId).Contains(_.Id));             
        }

        public async Task<IActionResult> CreateNewChat(ClaimsPrincipal curentUser, List<string> friendsId)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            var newChat = new Chat() { Name = "", DateCreate = DateTime.Now };
            var newChatBridges = dbContext.Users.Where(_ => friendsId.Contains(_.Id) || _.Id == user.Id).Select(_ => new ChatBridge() { User = _, Chat = newChat });
            newChat.Name = "Chat by " + String.Join(" ", newChatBridges.Select(_ => _.User.FirstName));
            await dbContext.Chats.AddAsync(newChat);
            await dbContext.ChatBridges.AddRangeAsync(newChatBridges);
            await dbContext.SaveChangesAsync();
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> UpdateChatName(ClaimsPrincipal curentUser, string chatId, string newName)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var chat = dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId));
            chat.Name = newName;
            dbContext.Entry(chat).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> AddNewMembers(ClaimsPrincipal curentUser, string chatId, List<string> friendsId)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var chat = dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId));
            var newChatBridges = dbContext.Users.Where(_ => friendsId.Contains(_.Id)).Select(_ => new ChatBridge() { User = _, Chat = chat });
            await dbContext.ChatBridges.AddRangeAsync(newChatBridges);
            await dbContext.SaveChangesAsync();
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> SendMessage(ClaimsPrincipal curentUser, string chatId, string textMessage)
        {
            var user = await _userManager.GetUserAsync(curentUser);

            // if chat id is null send message to general chat // DOTO: delete after release chat
            if (chatId == null)
            {
                await _chatHub.SendMessageForAll(textMessage);
                return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
            }

            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var chat = dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId));
            var newMessage = new Message() { Sender = user, Chat = chat, DateSend = DateTime.Now, Text = textMessage };
            await dbContext.Messages.AddRangeAsync(newMessage);
            await dbContext.SaveChangesAsync();
            await _chatHub.SendMessage(newMessage, dbContext.ChatBridges.Where(_ => _.ChatId == chat.Id).Select(_ => _.UserId).AsEnumerable());
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> GetMessageChunk(ClaimsPrincipal curentUser, string chatId, int chunkNumber)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.User.Id == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var messages = dbContext.Messages.AsEnumerable().Where(_ => _.ChatId == new Guid(chatId)).OrderBy(_ => _.DateSend).Where((_, i) => i >= (chunkNumber - 1) * MessageChunk.MessagesNumber && i < chunkNumber * MessageChunk.MessagesNumber).Select(_ => new { date = _.DateSend, text = _.Text, isCurrentUserMessage = _.SenderId == user.Id });
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult(messages);
        }
    }
}