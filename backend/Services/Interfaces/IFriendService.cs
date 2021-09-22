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
        (IEnumerable<FoundedUser> bestMatch, IEnumerable<FoundedUser> otherMatch) GetUsersByName(ClaimsPrincipal curentUser, string firstName, string lastName);
    }
}
