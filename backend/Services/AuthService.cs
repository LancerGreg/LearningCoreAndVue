using backend.Helpers.Interfaces;
using backend.Managers;
using backend.Models;
using backend.Repositories;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class AuthService : AppDbRepository, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthorizeAttribute _authorize;
        private readonly ISMTP smtp;

        public AuthService(IConfiguration configuration, AppDbContext dbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthorizeAttribute authorize, ISMTP smtp) : base(dbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _authorize = authorize;
            this.smtp = smtp;
        }

        public async Task<ActionAuthResult> SignIn(bool isValid, SignInUser modelUser)
        {
            if (isValid)
            {
                var result = await _signInManager.PasswordSignInAsync(modelUser.Email, modelUser.Password, true, true);
                if (result.Succeeded)
                {
                    _authorize.Authorization(modelUser.Email);
                    return new ActionAuthResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionAuthResult(ActionStatus.Error, result);
                }
            }
            return new ActionAuthResult(ActionStatus.Error);
        }
        public async Task<ActionAuthResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new ActionAuthResult(ActionStatus.Error);

            var encodeToken = Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Encode(token.Split('/').Select(_ => byte.Parse(_)).ToArray()).Replace('_', '/').Replace('-', '+') + "==";
            var result = await _userManager.ConfirmEmailAsync(user, encodeToken);
            if (result.Succeeded)
            {
                return new ActionAuthResult(ActionStatus.Success, result);
            }
            else
            {
                return new ActionAuthResult(ActionStatus.Error, result);
            }
        }

        public async Task<ActionAuthResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _authorize.LogoutAuthorization();
            return new ActionAuthResult(ActionStatus.Success);
        }

        public async Task<ActionAuthResult> ResetPassword(string email, string password, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ActionAuthResult(ActionStatus.Error);
            }

            var encodeToken = Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Encode(token.Split('/').Select(_ => byte.Parse(_)).ToArray()).Replace('_', '/').Replace('-', '+');
            var result = await _userManager.ResetPasswordAsync(user, encodeToken, password);
            if (result.Succeeded)
            {
                return new ActionAuthResult(ActionStatus.Success, result);
            }
            else
            {
                return new ActionAuthResult(ActionStatus.Error, result);
            }
        }

        public  async Task<ActionAuthResult> ResetPasswordRequest(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new ActionAuthResult(ActionStatus.Error);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var decodToken = String.Join("/", Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Decode(token).Select(_ => _.ToString()));
            smtp.SendResetPasswordRequest(email, decodToken);
            return new ActionAuthResult(ActionStatus.Success);
        }

        public async Task<ActionAuthResult> SignUp(bool isValid, SignUpUser modelUser)
        {
            if (isValid)
            {
                AppUser user = new AppUser { Email = modelUser.Email, UserName = modelUser.Email };
                var result = await _userManager.CreateAsync(user, modelUser.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var decodToken = String.Join("/", Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Decode(token).Select(_ => _.ToString()));
                    smtp.SendSignUpRequest(user.Email, decodToken);
                    return new ActionAuthResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionAuthResult(ActionStatus.Error, result);
                }
            }
            return new ActionAuthResult(ActionStatus.Error);
        }

        public bool UserIsAuthorized() => _authorize.OnAuthorization();
    }
}
