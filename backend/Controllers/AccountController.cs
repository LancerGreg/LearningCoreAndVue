using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using backend.Models;
using backend.Services;
using System;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("sign_up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUp([FromBody]SignUpUser modelUser) =>
            StatusCode((int)(await _accountService.SignUp(ModelState.IsValid, modelUser)));

        [HttpPost("sign_in")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignIn([FromBody]SignInUser modelUser) =>
            StatusCode((int)(await _accountService.SignIn(ModelState.IsValid, modelUser, !string.IsNullOrEmpty(modelUser.ReturnUrl) && Url.IsLocalUrl(modelUser.ReturnUrl))));

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout() =>
            StatusCode((int)(await _accountService.Logout()));
    }
}
