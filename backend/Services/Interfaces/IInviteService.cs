using backend.Managers;
using backend.Managers.ActionResult;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
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
        Task<IActionResult> InviteRequestById(ClaimsPrincipal curentUser, string userId);
        Task<IActionResult> InviteRequestByEmail(ClaimsPrincipal curentUser, string friendEmail);
        Task<IActionResult> ConfirmInvite(ClaimsPrincipal curentUser, string inviteId, Decide decide);
    }
}
