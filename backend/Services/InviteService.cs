using backend.Managers;
using backend.Models;
using backend.Repositories;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services
{
    public class InviteService : AppDbRepository, IInviteService
    {
        private readonly UserManager<AppUser> _userManager;

        public InviteService(AppDbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Invite>> GetAllInvites(ClaimsPrincipal curentUser)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            return dbContext.Invites.Where(_ => _.RecipientId == user.Id);
        }

        public async Task<IEnumerable<Invite>> GetNotDecideInvites(ClaimsPrincipal curentUser)
        {
            var invites = (await GetAllInvites(curentUser)).Where(_ => _.Decide == Decide.NotDecide).ToList();
            return (await GetAllInvites(curentUser)).Where(_ => _.Decide == Decide.NotDecide);
        }

        public async Task<ActionInviteResult> InviteRequest(ClaimsPrincipal curentUser, string friendId)
        {
            var newInvite = new Invite()
            {
                SenderId = (await _userManager.GetUserAsync(curentUser)).Id,
                RecipientId = (await dbContext.Users.FirstOrDefaultAsync(_ => _.Id == friendId)).Id,
                WhenSend = DateTime.Now,
                WhenDecide = null,
                Decide = Decide.NotDecide
            };

            dbContext.Invites.Add(newInvite);
            dbContext.SaveChanges();

            return new ActionInviteResult(ActionStatus.Success, "Friendship request is sent");
        }

        public async Task<ActionInviteResult> ConfirmInvite(ClaimsPrincipal curentUser, Guid inviteId, Decide decide)
        {
            var invite = await dbContext.Invites.FirstOrDefaultAsync(_ => _.Id == inviteId);
            invite.Decide = decide;
            invite.WhenDecide = DateTime.Now;

            dbContext.Entry(invite).State = EntityState.Modified;

            if (decide == Decide.Accept)
            {
                var senderFriendship = new Friendship() { AppUserId = invite.SenderId, FriendId = invite.RecipientId };
                var RecipientFriendship = new Friendship() { AppUserId = invite.RecipientId, FriendId = invite.SenderId };
                dbContext.Friendships.Add(senderFriendship);
                dbContext.Friendships.Add(RecipientFriendship);
            }

            dbContext.SaveChanges();

            return new ActionInviteResult(ActionStatus.Success, decide == Decide.Accept ? "Friendship request is accept" : "Friendship request is denie");
        }
    }
}
