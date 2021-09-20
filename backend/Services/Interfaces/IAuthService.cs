using backend.Managers;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
