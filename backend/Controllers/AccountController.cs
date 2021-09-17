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

        [HttpGet("user_is_authorized")]
        public JsonResult UserIsAuthorized() =>
            new JsonResult(_accountService.UserIsAuthorized());

        [HttpGet("user_credentiails")]
        public async Task<IActionResult> GetUserCredentilas() =>
            User.Identity.IsAuthenticated ? new JsonResult(await _accountService.GetUserCredentilas(User)) : BadRequest(new Error() { Code = "NotAuthenticated", Description = "User is not authenticated" });

        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token) =>
            GetActionResult(await _accountService.ConfirmEmail(email, token));

        [HttpPost("update_user")]
        public async Task<IActionResult> UpdateUser([FromBody]UserProfile userProfile) =>
            GetActionResult(await _accountService.UpdateUser(User, userProfile));

        [HttpPost("sign_up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUser modelUser) =>
            GetActionResult(await _accountService.SignUp(ModelState.IsValid, modelUser));

        [HttpPost("sign_in")]
        public async Task<IActionResult> SignIn([FromBody] SignInUser modelUser) =>
            GetActionResult(await _accountService.SignIn(ModelState.IsValid, modelUser));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() =>
            GetActionResult(await _accountService.Logout());

        [HttpPost("forgot_password_request")]
        public async Task<IActionResult> ResetPasswordRequest(string email) =>
            GetActionResult(await _accountService.ResetPasswordRequest(email));

        [HttpPost("confirm_reset_password")]
        public async Task<IActionResult> ConfirmResetPassword(string email, string passwrod, string token) =>
            GetActionResult(await _accountService.ResetPassword(email, passwrod, token));

        private IActionResult GetActionResult(backend.Managers.ActionResult actionResult)
        {
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
