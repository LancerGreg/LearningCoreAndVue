using backend.Managers;
using backend.Managers.ActionResult;
using backend.Managers.ActionResult.Responses;
using backend.Models;
using backend.Repositories;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend.Services
{
    public class TestingService : AppDbRepository, ITestingService
    {
        private static List<(int count, int fromPercent, int toPercent)> PercentageFriendsNumber = new List<(int, int, int)>()
        {
            (1, 1, 2), (2, 3, 4), (3, 5, 6), (4, 7, 8), (5, 9, 10),
            (6, 11, 13), (7, 14, 16), (8, 17, 19), (9, 20, 22), (10, 23, 26),
            (11, 27, 30), (12, 31, 34), (13, 35, 39), (14, 40, 45), (15, 46, 54),
            (16, 55, 60), (17, 61, 65), (18, 66, 69), (19, 70, 73), (20, 74, 77),
            (21, 78, 80), (22, 81, 83), (23, 84, 86), (24, 87, 89), (25, 90, 91),
            (26, 92, 93), (27, 94, 95), (28, 96, 97), (29, 98, 99), (30, 100, 100),
        };

        private readonly UserManager<AppUser> _userManager;
        private readonly IFriendService _friendService;

        public TestingService(AppDbContext dbContext, UserManager<AppUser> userManager, IFriendService friendService) : base(dbContext)
        {
            _userManager = userManager;
            _friendService = friendService;
        }

        public async Task<IActionResult> CreateTestUsers(int count)
        {
            var index = dbContext.Users.AsEnumerable().Where(_ => _.Email.Contains("test")).Max(_ => Int32.Parse(Regex.Match(_.Email, @"\d+").Value)) + 1;
            for (int i = 0; i < count; i++)
                await _userManager.CreateAsync(new AppUser() { Email = "test" + (index + i) + "@test.com", EmailConfirmed = true, FirstName = "test" + (index + i) + "@test.com", UserName = "test" + (index + i) + "@test.com" }, "1q2w3e$R5t");

            return new ActionAuthResult(ActionStatus.Success, TestingResponse.Success()).GetActionResult();
        }

        public async Task<IActionResult> CreateTestFriendships(int from, int? countUsers)
        {
            var random = new Random();
            var usersId = dbContext.Users.AsEnumerable().Where(_ => _.Email.Contains("test") && Int32.Parse(Regex.Match(_.Email, @"\d+").Value) > from).Select(_ => _.Id);
            
            if (countUsers.HasValue)
                usersId.Take(countUsers.Value);

            foreach (var id in usersId)
            {
                await AddFriendship(usersId, id, random);
            }

            return new ActionAuthResult(ActionStatus.Success, TestingResponse.Success()).GetActionResult();
        }

        private async Task AddFriendship(IEnumerable<string> usersId, string id, Random random)
        {
            var randomPercent = random.Next(99) + 1;
            int countFriend = PercentageFriendsNumber.FirstOrDefault(_ => _.fromPercent <= randomPercent && _.toPercent >= randomPercent).count - _friendService.GetUserFriends(id).Count();
            for (int i = 0; i < countFriend; i++)
            {
                var invite = new Invite()
                {
                    SenderId = id,
                    RecipientId = GetRandomFutureFriendId(usersId, id, random),
                    WhenSend = DateTime.Now,
                    WhenDecide = DateTime.Now,
                    Decide = Decide.Accept,
                };

                var senderFriendship = new Friendship() { AppUserId = invite.SenderId, FriendId = invite.RecipientId };
                var RecipientFriendship = new Friendship() { AppUserId = invite.RecipientId, FriendId = invite.SenderId };

                await dbContext.Invites.AddAsync(invite);
                await dbContext.Friendships.AddAsync(senderFriendship);
                await dbContext.Friendships.AddAsync(RecipientFriendship);

                await dbContext.SaveChangesAsync();
            }
        }

        private string GetRandomFutureFriendId(IEnumerable<string> usersId, string currentUserId, Random random) => 
            usersId.Where(id => id != currentUserId && !dbContext.Friendships.AsEnumerable().Any(fs => fs.AppUserId == id)).ElementAt(random.Next(usersId.Count()));
    }
}
