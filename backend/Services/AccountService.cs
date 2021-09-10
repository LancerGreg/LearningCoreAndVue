using backend.Managers;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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

        public async Task<ActionResult> SignUp(bool isValid, SignUpUser modelUser)
        {
            if (isValid)
            {
                AppUser user = new AppUser { Email = modelUser.Email, UserName = modelUser.Email };
                var result = await _userManager.CreateAsync(user, modelUser.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return new ActionResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionResult(ActionStatus.Error, result);
                }
            }
            return new ActionResult(ActionStatus.Error);
        }

        public async Task<ActionResult> SignIn(bool isValid, SignInUser modelUser)
        {
            if (isValid)
            {
                var result = await _signInManager.PasswordSignInAsync(modelUser.Email, modelUser.Password, modelUser.RememberMe, true);
                if (result.Succeeded)
                {
                    return new ActionResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionResult(ActionStatus.Error, result);
                }
            }
            return new ActionResult(ActionStatus.Error);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new ActionResult(ActionStatus.Success);
        }

        public async Task<object> GetUserCredentilas(ClaimsPrincipal user) => await _userManager.GetUserAsync(user);
    }
}
