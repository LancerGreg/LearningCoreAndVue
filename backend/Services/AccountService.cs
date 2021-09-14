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
using backend.Repositories;
using backend.Helpers;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace backend.Services
{
    public class AccountService : AppDbRepository, IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthorizeAttribute _authorize;
        private readonly ISMTP smtp;

        public AccountService(IConfiguration configuration, AppDbContext dbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthorizeAttribute authorize, ISMTP smtp) : base(dbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _authorize = authorize;
            this.smtp = smtp;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = dbContext.Users.FirstOrDefault(_ => _.Email == model.Email);

            if (user == null)
            {
                // todo: need to add logger
                return null;
            }

            var token = _configuration.GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<ActionResult> SignUp(bool isValid, SignUpUser modelUser)
        {
            if (isValid)
            {
                AppUser user = new AppUser { Email = modelUser.Email, UserName = modelUser.Email };
                var result = await _userManager.CreateAsync(user, modelUser.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    smtp.SendSignUpRequest(user.Email, token);
                    return new ActionResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionResult(ActionStatus.Error, result);
                }
            }
            return new ActionResult(ActionStatus.Error);
        }

        public async Task<ActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new ActionResult(ActionStatus.Error);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new ActionResult(ActionStatus.Success, result);
            }
            else
            {
                return new ActionResult(ActionStatus.Error, result);
            }
        }

        public async Task<ActionResult> SignIn(bool isValid, SignInUser modelUser)
        {
            if (isValid)
            {
                var result = await _signInManager.PasswordSignInAsync(modelUser.Email, modelUser.Password, true, true);
                if (result.Succeeded)
                {
                    _authorize.Authorization(modelUser.Email);
                    return new ActionResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionResult(ActionStatus.Error, result);
                }
            }
            return new ActionResult(ActionStatus.Error);
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _authorize.LogoutAuthorization();
            return new ActionResult(ActionStatus.Success);
        }

        public async Task<object> GetUserCredentilas(ClaimsPrincipal user) => await _userManager.GetUserAsync(user);

        public AppUser GetUser(string id) => dbContext.Users.FirstOrDefault(_ => _.Id == id);

        public bool UserIsAuthorized() => _authorize.OnAuthorization();
    }
}
