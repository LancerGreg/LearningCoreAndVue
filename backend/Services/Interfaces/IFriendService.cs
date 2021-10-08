using backend.Managers;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface IFriendService
    {
        Task<IEnumerable<AppUser>> GetFriends(ClaimsPrincipal curentUser);
        IEnumerable<AppUser> GetUserFriends(string userId);
        Task<AppUser> GetUserById(string userId);
        Task<IEnumerable<FoundedUser>> GetUserByEmail(ClaimsPrincipal curentUser, string userEmail);
        Task<IEnumerable<FoundedUser>> GetUserByPhone(ClaimsPrincipal curentUser, string userPhone);
        Task<(IEnumerable<FoundedUser> bestMatch, IEnumerable<FoundedUser> otherMatch)> GetUsersByName(ClaimsPrincipal curentUser, string firstName, string lastName);
        Task<object> GetGraphData(ClaimsPrincipal curentUser, int range, bool simplifiedLink);
        Task<object> GetGraphData(ClaimsPrincipal curentUser, int range, bool simplifiedLink, string friendId);
    }
}
