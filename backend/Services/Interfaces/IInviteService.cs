using backend.Managers;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface IInviteService
    {
        Task<IEnumerable<Invite>> GetAllInvites(ClaimsPrincipal curentUser);
        Task<IEnumerable<Invite>> GetNotDecideInvites(ClaimsPrincipal curentUser);
        Task<ActionInviteResult> InviteRequest(ClaimsPrincipal curentUser, string friendId);
        Task<ActionInviteResult> ConfirmInvite(ClaimsPrincipal curentUser, Guid inviteId, Decide decide);
    }
}
