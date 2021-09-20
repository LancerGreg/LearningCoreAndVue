using backend.Managers;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("not_decide_invites")]
        public async Task<JsonResult> GetNotDecideInvites() =>
            new JsonResult(await _inviteService.GetNotDecideInvites(User));

        [HttpPost("invite_request")]
        public async Task<IActionResult> InviteRequest(string friendId) =>
            GetActionResult(await _inviteService.InviteRequest(User, friendId));

        [HttpPost("confirm_invite")]
        public async Task<IActionResult> ConfirmInvite(Guid inviteId, Decide decide) =>
            GetActionResult(await _inviteService.ConfirmInvite(User, inviteId, decide));

        private IActionResult GetActionResult(backend.Managers.ActionInviteResult actionResult)
        {
            if (actionResult._actionStatus == ActionStatus.Success)
            {
                return Ok(actionResult.actionMessage);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
