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

        public async Task<IEnumerable<object>> GetChatsByCurrentUser(ClaimsPrincipal curentUser)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            var chats = dbContext.Chats.Where(_ => dbContext.ChatBridges.Where(cb => cb.UserId == user.Id).Select(cb => cb.ChatId).Contains(_.Id)).AsEnumerable();
            return chats.GroupJoin(dbContext.Messages.AsEnumerable(),
                                   chat => chat.Id,
                                   message => message.ChatId,
                                   (chat, messages) => new
                                   {
                                       chat = chat, 
                                       lastMessage = messages.Any() 
                                                     ? messages.OrderByDescending(_ => _.DateSend).Select(_ => new { text = _.Text, dateSend = _.DateSend.ToString("yyyy/MM/dd, HH:MM:ss") }).FirstOrDefault()
                                                     : new { text = "", dateSend = chat.DateCreate.ToString("yyyy/MM/dd, HH:MM:ss") },
                                       listMessages = new List<Message>(),
                                   }).OrderByDescending(_ => _.lastMessage.dateSend);
        }

        public async Task<IActionResult> CreateNewChat(ClaimsPrincipal curentUser, string chatName)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            var newChat = new Chat() 
            { 
                Name = chatName == null || chatName.Trim() == "" ? "Chat by " + user.FirstName : chatName,
                DateCreate = DateTime.Now 
            };
            var newChatBridge = new ChatBridge() { User = user, Chat = newChat };
            await dbContext.Chats.AddAsync(newChat);
            await dbContext.ChatBridges.AddAsync(newChatBridge);
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

            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var chat = dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId));
            var newMessage = new Message() { Sender = user, Chat = chat, DateSend = DateTime.Now, Text = textMessage };
            await dbContext.Messages.AddAsync(newMessage);
            await dbContext.SaveChangesAsync();
            await _chatHub.SendMessage(newMessage, dbContext.ChatBridges.Where(_ => _.ChatId == chat.Id).Select(_ => _.UserId).AsEnumerable());
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> GetMessages(ClaimsPrincipal curentUser, string chatId)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var messages = dbContext.Messages.AsEnumerable().Where(_ => _.ChatId == new Guid(chatId)).OrderByDescending(_ => _.DateSend).Select(_ => new { id = _.Id, date = _.DateSend.ToString("yyyy/MM/dd, HH:MM:ss"), text = _.Text, isCurrentUserMessage = _.SenderId == user.Id }).OrderBy(_ => _.date);
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult(messages);
        }
    }
}