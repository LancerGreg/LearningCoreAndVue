using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
            await _authService.SignIn(ModelState.IsValid, modelUser);

        [HttpGet("user_is_authorized")]
        public JsonResult UserIsAuthorized() =>
            new JsonResult(_authService.UserIsAuthorized());

        [HttpPost("sign_up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUser modelUser) =>
            await _authService.SignUp(ModelState.IsValid, modelUser);

        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token) =>
            await _authService.ConfirmEmail(email, token);

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() =>
            await _authService.Logout();

        [HttpPost("forgot_password_request")]
        public async Task<IActionResult> ResetPasswordRequest(string email) =>
            await _authService.ResetPasswordRequest(email);

        [HttpPost("confirm_reset_password")]
        public async Task<IActionResult> ConfirmResetPassword(string email, string passwrod, string token) =>
            await _authService.ResetPassword(email, passwrod, token);

        [HttpPost("reset_number_phone")]
        public async Task<IActionResult> ResetNumberPhone(string NumberPhone) =>
            await _authService.ResetNumberPhone(User, NumberPhone);

        [HttpPost("confirm_reset_number_phone")]
        public async Task<IActionResult> ConfirmResetNumberPhone(string token) =>
            await _authService.ConfirmResetNumberPhone(User, token);
    }
}
