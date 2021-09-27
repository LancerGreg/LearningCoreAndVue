using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using backend.Models;
using backend.Services;
using System;
using Microsoft.AspNetCore.Http;
using backend.Managers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using backend.Services.Interfaces;

namespace backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("user_credentiails")]
        public async Task<IActionResult> GetUserCredentilas() =>
            User.Identity.IsAuthenticated ? new JsonResult(await _accountService.GetUserCredentilas(User)) : BadRequest(new Error() { Code = "NotAuthenticated", Description = "User is not authenticated" });

        [HttpPost("update_user")]
        public async Task<IActionResult> UpdateUser([FromBody]UserProfile userProfile) =>
            GetActionResult(await _accountService.UpdateUser(User, userProfile));

        private IActionResult GetActionResult(backend.Managers.ActionAccountResult actionResult)
        {
            if (actionResult._actionStatus == ActionStatus.Success)
            {
                return actionResult.message != "" ? Ok(actionResult.message) : Ok(ActionStatus.Success.ToString());
            }
            else
            {
                if (actionResult._identityResult != null)
                {
                    return BadRequest(actionResult.GetErrorList(actionResult._identityResult));
                }
                return BadRequest();
            }
        }
    }
}
