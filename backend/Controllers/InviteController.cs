using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/invite")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IInviteService _inviteService;

        public InviteController(IInviteService inviteService)
        {
            _inviteService = inviteService;
        }

        [HttpGet("get_not_decide_invites")]
        public async Task<JsonResult> GetNotDecideInvites() =>
            new JsonResult(await _inviteService.GetNotDecideInvites(User));

        [HttpGet("get_not_decide_invites_count")]
        public async Task<JsonResult> GetNotDecideInvitesCount() =>
            new JsonResult(await _inviteService.GetNotDecideInvitesCount(User));

        [HttpPost("invite_request_by_id")]
        public async Task<IActionResult> InviteRequest(string userId) =>
            await _inviteService.InviteRequestById(User, userId);

        [HttpPost("invite_request_by_email")]
        public async Task<IActionResult> InviteRequestByEmail(string friendEmail) =>
            await _inviteService.InviteRequestByEmail(User, friendEmail);

        [HttpPost("confirm_invite")]
        public async Task<IActionResult> ConfirmInvite(string inviteId, Decide decide) =>
            await _inviteService.ConfirmInvite(User, inviteId, decide);
    }
}
