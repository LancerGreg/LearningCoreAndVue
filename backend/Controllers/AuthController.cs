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
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign_in")]
        public async Task<IActionResult> SignIn([FromBody] SignInUser modelUser) =>
            GetActionResult(await _authService.SignIn(ModelState.IsValid, modelUser));

        [HttpGet("user_is_authorized")]
        public JsonResult UserIsAuthorized() =>
            new JsonResult(_authService.UserIsAuthorized());

        [HttpPost("sign_up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUser modelUser) =>
            GetActionResult(await _authService.SignUp(ModelState.IsValid, modelUser));

        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token) =>
            GetActionResult(await _authService.ConfirmEmail(email, token));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() =>
            GetActionResult(await _authService.Logout());

        [HttpPost("forgot_password_request")]
        public async Task<IActionResult> ResetPasswordRequest(string email) =>
            GetActionResult(await _authService.ResetPasswordRequest(email));

        [HttpPost("confirm_reset_password")]
        public async Task<IActionResult> ConfirmResetPassword(string email, string passwrod, string token) =>
            GetActionResult(await _authService.ResetPassword(email, passwrod, token));

        private IActionResult GetActionResult(backend.Managers.ActionAuthResult actionResult)
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
