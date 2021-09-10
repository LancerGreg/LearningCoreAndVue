using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using backend.Models;
using backend.Services;
using System;
using Microsoft.AspNetCore.Http;
using backend.Managers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [HttpGet("user_is_login")]
        public JsonResult UserIsLogin() =>
            new JsonResult(User.Identity.IsAuthenticated);

        [HttpGet("user_credentiails")]
        public async Task<IActionResult> UserCredentilas() =>
            User.Identity.IsAuthenticated ? new JsonResult(await _accountService.GetUserCredentilas(User)) : BadRequest(new Error() { Code = "NotAuthenticated", Description = "User is not authenticated" });

        [HttpPost("sign_up")]
        public async Task<IActionResult> SignUp([FromBody]SignUpUser modelUser) =>
            GetActionResult(await _accountService.SignUp(ModelState.IsValid, modelUser));

        [HttpPost("sign_in")]
        public async Task<IActionResult> SignIn([FromBody]SignInUser modelUser) =>
            GetActionResult(await _accountService.SignIn(ModelState.IsValid, modelUser));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() =>
            GetActionResult(await _accountService.Logout());

        private IActionResult GetActionResult(backend.Managers.ActionResult actionResult) {
            if (actionResult._actionStatus == ActionStatus.Success)
            {
                return Ok(ActionStatus.Success.ToString());
            }
            else
            {
                if (actionResult._identityResult != null)
                {
                    return BadRequest(actionResult.GetErrorList(actionResult._identityResult));
                }
                if (actionResult._signInResult != null)
                {
                    return BadRequest(actionResult.GetErrorList(actionResult._signInResult));
                }
                return BadRequest();
            }
        }
    }
}
