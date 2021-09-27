using backend.Managers;
using backend.Models;
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
        Task<ActionAuthResult> SignIn(bool isValid, SignInUser modelUser);
        bool UserIsAuthorized();
        Task<ActionAuthResult> SignUp(bool isValid, SignUpUser modelUser);
        Task<ActionAuthResult> ConfirmEmail(string email, string token);
        Task<ActionAuthResult> Logout();
        Task<ActionAuthResult> ResetPasswordRequest(string email);
        Task<ActionAuthResult> ResetPassword(string email, string password, string token);
        Task<ActionAuthResult> ResetNumberPhone(ClaimsPrincipal curentUser, string NumberPhone);
        Task<ActionAuthResult> ConfirmResetNumberPhone(ClaimsPrincipal curentUser, string token);
    }
}
