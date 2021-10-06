using backend.Managers;
using backend.Managers.ActionResult;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio.AspNet.Common;

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
