using backend.Managers.ActionResult.Responses;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            User.Identity.IsAuthenticated
                ? new JsonResult(await _accountService.GetUserCredentilas(User))
                : BadRequest(BaseResponse.NotAuthenticated());

        [HttpPost("update_user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfile userProfile) =>
            await _accountService.UpdateUser(User, userProfile);
    }
}
