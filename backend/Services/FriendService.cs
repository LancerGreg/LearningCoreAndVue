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

        public IEnumerable<AppUser> GetFriends(string friendId) => dbContext.Users.Where(_ => dbContext.Friendships.Where(_ => _.AppUserId == friendId).Select(_ => _.FriendId).Contains(_.Id));
    }
}
