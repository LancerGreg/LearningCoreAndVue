using backend.Models;
using backend.Models.ResultStatus;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<SignUpStatus> SignUp (bool isValid, SignUpUser modelUser)
        {
            if (isValid)
            {
                AppUser user = new AppUser { Email = modelUser.Email, UserName = modelUser.Email };
                var result = await _userManager.CreateAsync(user, modelUser.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return SignUpStatus.Success;
                }
                else
                {
                    return SignUpStatus.Error;
                }
            }
            return SignUpStatus.Success;
        }

        public async Task<SignInStatus> SignIn(bool isValid, SignInUser modelUser, bool returnUrl)
        {
            if (isValid)
            {
                var result = await _signInManager.PasswordSignInAsync(modelUser.Email, modelUser.Password, modelUser.RememberMe, false);
                if (result.Succeeded)
                {
                    return returnUrl ? SignInStatus.Redirect : SignInStatus.Success;
                }
                else
                {
                    return SignInStatus.InvalidLogin;
                }
            }
            return SignInStatus.Success;
        }

        public async Task<LogoutStatus> Logout()
        {
            await _signInManager.SignOutAsync();
            return LogoutStatus.Success;
        }
    }
}
