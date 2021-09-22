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
        Task<IEnumerable<InviteTable>> GetNotDecideInvites(ClaimsPrincipal curentUser);
        Task<int> GetNotDecideInvitesCount(ClaimsPrincipal curentUser);
        Task<ActionInviteResult> InviteRequestById(ClaimsPrincipal curentUser, string userId);
        Task<ActionInviteResult> InviteRequestByEmail(ClaimsPrincipal curentUser, string friendEmail);
        Task<ActionInviteResult> ConfirmInvite(ClaimsPrincipal curentUser, string inviteId, Decide decide);
    }
}
