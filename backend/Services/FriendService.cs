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
            return dbContext.Users.Where(_ => dbContext.Friendships.Where(_ => _.AppUserId == user.Id).Select(_ => _.FriendId).Contains(_.Id));
        }

        public IEnumerable<AppUser> GetUserFriends(string userId) => 
            dbContext.Users.Where(_ => dbContext.Friendships.Where(_ => _.AppUserId == userId).Select(_ => _.FriendId).Contains(_.Id));

        public async Task<AppUser> GetUserById(string userId) =>
            await dbContext.Users.FirstOrDefaultAsync(_ => _.Id == userId);

        public async Task<AppUser> GetUserByEmail(string userEmail) =>
            await dbContext.Users.FirstOrDefaultAsync(_ => _.Email == userEmail);

        public (IEnumerable<FoundedUser> bestMatch, IEnumerable<FoundedUser> otherMatch) GetUsersByName(ClaimsPrincipal curentUser, string firstName, string lastName)
        {
            var users = dbContext.Users.Where(_ => _.FirstName.ToLower().Contains(firstName) || _.LastName.ToLower().Contains(lastName));
            var bestMatch = users.Where(_ => _.FirstName.ToLower().Contains(firstName) && _.LastName.ToLower().Contains(lastName)).OrderBy(_ => _.FirstName).ThenBy(_ => _.LastName);
            var otherMatch = users.Where(_ => !(bestMatch.Any(bm => bm.Id == _.Id))).OrderBy(_ => _.FirstName).ThenBy(_ => _.LastName);
            var invites = dbContext.Invites.Where(_ => _.SenderId == _userManager.GetUserId(curentUser));
            var friendShip = dbContext.Friendships.Where(_ => _.AppUserId == _userManager.GetUserId(curentUser));

            return (bestMatch.Select(_ => new FoundedUser(_) { IsFriend = friendShip.Any(fs => fs.FriendId == _.Id), HaveInvite = invites.Any(i => i.RecipientId == _.Id) }), otherMatch.Select(_ => new FoundedUser(_)));
        }

        public async Task<AppUser> GetUserByPhone(string userPhone) =>
            await dbContext.Users.FirstOrDefaultAsync(_ => _.PhoneNumber == userPhone);
    }
}
