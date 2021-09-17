using backend.Managers;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IAccountService
    {
        Task<ActionResult> SignUp(bool isValid, SignUpUser modelUser);
        Task<ActionResult> SignIn(bool isValid, SignInUser modelUser);
        Task<ActionResult> Logout();
        Task<ActionResult> ResetPasswordRequest(string email);
        Task<ActionResult> ResetPassword(string email, string password, string token);
        Task<UserProfile> GetUserCredentilas(ClaimsPrincipal user);
        AppUser GetUser(string id);
        bool UserIsAuthorized();
        Task<ActionResult> ConfirmEmail(string email, string token);
        Task<ActionResult> UpdateUser(ClaimsPrincipal claimsPrincipal, UserProfile userProfile);
    }
}
