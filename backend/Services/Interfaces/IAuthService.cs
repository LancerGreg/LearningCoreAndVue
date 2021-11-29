using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IActionResult> SignIn(bool isValid, SignInUser modelUser);
        bool UserIsAuthorized();
        Task<IActionResult> SignUp(bool isValid, SignUpUser modelUser);
        Task<IActionResult> ConfirmEmail(string email, string token);
        Task<IActionResult> Logout();
        Task<IActionResult> ResetPasswordRequest(string email);
        Task<IActionResult> ResetPassword(string email, string password, string token);
        Task<IActionResult> ResetNumberPhone(ClaimsPrincipal curentUser, string NumberPhone);
        Task<IActionResult> ConfirmResetNumberPhone(ClaimsPrincipal curentUser, string token);
    }
}
