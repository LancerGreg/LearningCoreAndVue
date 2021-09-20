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
        IEnumerable<AppUser> GetFriends(string friendId);
    }
}
