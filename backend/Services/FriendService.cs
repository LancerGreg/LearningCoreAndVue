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
    public class FriendService : AppDbRepository, IFriendService
    {
        private readonly UserManager<AppUser> _userManager;

        public FriendService(AppDbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AppUser>> GetFriends(ClaimsPrincipal curentUser)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            return dbContext.Users.Where(_ => _.Id != user.Id && dbContext.Friendships.Where(fs => fs.AppUserId == user.Id).Select(_ => _.FriendId).Contains(_.Id));
        }

        public IEnumerable<AppUser> GetUserFriends(string userId) =>
            dbContext.Users.Where(_ => dbContext.Friendships.Where(fs => fs.AppUserId == userId).Select(_ => _.FriendId).Contains(_.Id));

        public async Task<AppUser> GetUserById(string userId) =>
            await dbContext.Users.FirstOrDefaultAsync(_ => _.Id == userId);

        public async Task<IEnumerable<FoundedUser>> GetUserByEmail(ClaimsPrincipal curentUser, string userEmail)
        {
            var user = await _userManager.GetUserAsync(curentUser);

            var invites = dbContext.Invites.Where(_ => _.SenderId == user.Id);
            var friendships = dbContext.Friendships.Where(_ => _.AppUserId == user.Id);

            return dbContext.Users.Where(_ => _.Id != user.Id && _.Email.ToLower().Contains(userEmail)).Select(_ => new FoundedUser(_) { IsFriend = friendships.Any(fs => fs.FriendId == _.Id), HaveInvite = invites.Any(i => i.RecipientId == _.Id) });
        }

        public async Task<(IEnumerable<FoundedUser> bestMatch, IEnumerable<FoundedUser> otherMatch)> GetUsersByName(ClaimsPrincipal curentUser, string firstName, string lastName)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            var users = dbContext.Users.Where(_ => _.Id != user.Id && (_.FirstName.ToLower().Contains(firstName) || _.LastName.ToLower().Contains(lastName)));
            var bestMatch = users.Where(_ => _.FirstName.ToLower().Contains(firstName) && _.LastName.ToLower().Contains(lastName)).OrderBy(_ => _.FirstName).ThenBy(_ => _.LastName);
            var otherMatch = users.Where(_ => !(bestMatch.Any(bm => bm.Id == _.Id))).OrderBy(_ => _.FirstName).ThenBy(_ => _.LastName);
            var invites = dbContext.Invites.Where(_ => _.SenderId == user.Id);
            var friendships = dbContext.Friendships.Where(_ => _.AppUserId == user.Id);

            return (
                bestMatch.Select(_ => new FoundedUser(_) { IsFriend = friendships.Any(fs => fs.FriendId == _.Id), HaveInvite = invites.Any(i => i.RecipientId == _.Id) }),
                otherMatch.Select(_ => new FoundedUser(_) { IsFriend = friendships.Any(fs => fs.FriendId == _.Id), HaveInvite = invites.Any(i => i.RecipientId == _.Id) })
            );
        }

        public async Task<IEnumerable<FoundedUser>> GetUserByPhone(ClaimsPrincipal curentUser, string userPhone)
        {
            var user = await _userManager.GetUserAsync(curentUser);

            var invites = dbContext.Invites.Where(_ => _.SenderId == user.Id);
            var friendships = dbContext.Friendships.Where(_ => _.AppUserId == user.Id);

            return dbContext.Users.Where(_ => _.Id != user.Id && _.PhoneNumber.Contains(userPhone.Trim())).Select(_ => new FoundedUser(_) { IsFriend = friendships.Any(fs => fs.FriendId == _.Id), HaveInvite = invites.Any(i => i.RecipientId == _.Id) });
        }

        public async Task<object> GetGraphData(ClaimsPrincipal curentUser, int range, bool simplifiedLink)
        {
            var user = await _userManager.GetUserAsync(curentUser);
            return GraphData(user, user, range, simplifiedLink);
        }

        public async Task<object> GetGraphData(ClaimsPrincipal curentUser, int range, bool simplifiedLink, string friendId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(_ => _.Id == friendId);
            return GraphData(user, await _userManager.GetUserAsync(curentUser), range, simplifiedLink);
        }

        private object GraphData(AppUser user, AppUser curentUser, int range, bool simplifiedLink)
        {
            var users = new List<AppUser>() { user };
            var friendships = new List<Friendship>();
            friendships.AddRange(dbContext.Friendships.Where(_ => _.AppUserId == user.Id));
            for (int i = 0; i < range; i++)
            {
                if (simplifiedLink)
                {
                    friendships.AddRange(dbContext.Friendships.AsEnumerable().Where(_ => users.Any(u => u.Id == _.AppUserId) && !friendships.Any(f => f.Id == _.Id)));
                    users.AddRange(dbContext.Users.AsEnumerable().Where(_ => !users.Any(u => u.Id == _.Id) && friendships.Any(f => f.FriendId == _.Id)));
                }
                else
                {
                    users.AddRange(dbContext.Users.AsEnumerable().Where(_ => !users.Any(u => u.Id == _.Id) && friendships.Any(f => f.FriendId == _.Id)));
                    friendships.AddRange(dbContext.Friendships.AsEnumerable().Where(_ => users.Any(u => u.Id == _.AppUserId) && !friendships.Any(f => f.Id == _.Id)));
                }
            }

            return
            (
                users.Select(_ => new { Id = _.Id, FullName = _.FirstName + " " + _.LastName, IsFriend = friendships.Any(fs => (fs.AppUserId == curentUser.Id && fs.FriendId == _.Id) || (fs.AppUserId == _.Id && fs.FriendId == curentUser.Id)), HaveInvite = dbContext.Invites.Any(i => (i.SenderId == curentUser.Id && i.RecipientId == _.Id) || (i.SenderId == _.Id && i.RecipientId == curentUser.Id)) }),
                friendships.Where(_ => users.Any(u => u.Id == _.AppUserId) && users.Any(u => u.Id == _.FriendId)).Select(_ => new { Id = _.Id, FirstUserId = _.AppUserId, SecondUserId = _.FriendId })
            );
        }
    }
}
