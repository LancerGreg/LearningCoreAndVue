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

        public async Task<IEnumerable<ChatData>> GetChatsByCurrentUser(ClaimsPrincipal curentUser)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            var chats = dbContext.Chats.Where(_ => dbContext.ChatBridges.Where(cb => cb.UserId == user.Id).Select(cb => cb.ChatId).Contains(_.Id)).AsEnumerable();
            return chats.GroupJoin(
                dbContext.Messages.AsEnumerable(),
                chat => chat.Id,
                message => message.ChatId,
                (chat, messages) => new ChatData
                {
                    chat = chat, 
                    lastMessage = messages.Any() 
                        ? messages.OrderByDescending(_ => _.DateSend).Select(_ => new LastMessage() { text = _.Text, dateSend = _.DateSend.ToString("yyyy/MM/dd, HH:mm:ss") }).FirstOrDefault()
                        : new LastMessage() { text = "", dateSend = chat.DateCreate.ToString("yyyy/MM/dd, HH:mm:ss") },
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

            if (newName == null || newName == "")
                return new ActionChatResult(ActionStatus.Error, ChatResponse.EmptyValue()).GetActionResult();

            if (newName.Length > 64)
                return new ActionChatResult(ActionStatus.Error, ChatResponse.LongValue()).GetActionResult();

            var chat = dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId));
            chat.Name = newName;
            dbContext.Entry(chat).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            await _chatHub.RenameChatName(dbContext.ChatBridges.Where(_ => _.ChatId == new Guid(chatId)).Select(_ => _.UserId), chat);
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> AddNewMembers(ClaimsPrincipal curentUser, string chatId, string memberId)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var chat = dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId));
            var newMember = dbContext.Users.FirstOrDefault(_ => _.Id == memberId);
            var newChatBridges = new ChatBridge() { Chat = chat, User = newMember };
            await dbContext.ChatBridges.AddRangeAsync(newChatBridges);
            await dbContext.SaveChangesAsync();
            await _chatHub.AddToChat(memberId, (await GetChatsByCurrentUser(curentUser)).FirstOrDefault(_ => _.chat.Id == new Guid(chatId)));
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
            await _chatHub.SendMessage(newMessage, dbContext.ChatBridges
                .Where(_ => _.ChatId == chat.Id).Where(_ => _.UserId != user.Id).Select(_ => _.UserId).AsEnumerable());
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> GetMessages(ClaimsPrincipal curentUser, string chatId)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var messages = dbContext.Messages.AsEnumerable().Where(_ => _.ChatId == new Guid(chatId)).OrderByDescending(_ => _.DateSend).Select(_ => new { id = _.Id, date = _.DateSend.ToString("yyyy/MM/dd, HH:mm:ss"), text = _.Text, isCurrentUserMessage = _.SenderId == user.Id }).OrderBy(_ => _.date);
            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult(messages);
        }

        public async Task<IActionResult> LeaveChat(ClaimsPrincipal curentUser, string chatId)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var chatBridges = dbContext.ChatBridges.Where(_ => _.ChatId == new Guid(chatId)).ToList();

            dbContext.ChatBridges.Remove(chatBridges.FirstOrDefault(_ => _.UserId == user.Id));
            if (chatBridges.Count() == 1)
            {
                dbContext.Messages.RemoveRange(dbContext.Messages.Where(_ => _.ChatId == new Guid(chatId)));
                dbContext.Chats.Remove(dbContext.Chats.FirstOrDefault(_ => _.Id == new Guid(chatId)));
            }

            await dbContext.SaveChangesAsync();

            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> GetUsersByName(ClaimsPrincipal curentUser, string chatId, string fullName)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            if (!dbContext.ChatBridges.Any(_ => _.UserId == user.Id && _.ChatId == new Guid(chatId)))
                return new ActionChatResult(ActionStatus.Error, ChatResponse.Forbidden()).GetActionResult();

            var users = dbContext.Users.AsEnumerable().Where(_ => _.Id != user.Id && (_.FullName.ToLower().Contains(fullName.ToLower())));

            // this LINQ query is hard to write in lambda performance for multi join
            var friends = (from u in users
                          join fs in dbContext.Friendships.AsEnumerable().Where(_ => _.AppUserId == user.Id) on u.Id equals fs.FriendId
                          join cb in dbContext.ChatBridges.AsEnumerable().Where(_ => _.ChatId == new Guid(chatId)) on u.Id equals cb.UserId into _cb from subcb in _cb.DefaultIfEmpty()
                          select new { Id = u.Id, FullName = u.FullName, AlreadyInChat = subcb != null })
                          .OrderBy(_ => _.AlreadyInChat).ThenBy(_ => _.FullName);

            return new ActionChatResult(ActionStatus.Success, ChatResponse.Success()).GetActionResult(friends);
        }
    }
}