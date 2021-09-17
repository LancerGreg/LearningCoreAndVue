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
using backend.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;

namespace backend.Services
{
    public class AccountService : AppDbRepository, IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthorizeAttribute _authorize;
        private readonly ISMTP smtp;
        private readonly IHttpContextAccessor context;

        public AccountService(IConfiguration configuration, AppDbContext dbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthorizeAttribute authorize, ISMTP smtp, IHttpContextAccessor context) : base(dbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _authorize = authorize;
            this.smtp = smtp;
            this.context = context;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = dbContext.Users.FirstOrDefault(_ => _.Email == model.Email);

            if (user == null) return null;

            return new AuthenticateResponse(user, _configuration.GenerateJwtToken(user));
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
                    var decodToken = String.Join("/", Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Decode(token).Select(_ => _.ToString()));
                    smtp.SendSignUpRequest(user.Email, decodToken);
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

            var encodeToken = Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Encode(token.Split('/').Select(_ => byte.Parse(_)).ToArray()).Replace('_','/').Replace('-','+') + "==";
            var result = await _userManager.ConfirmEmailAsync(user, encodeToken);
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

        public async Task<ActionResult> ResetPasswordRequest(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new ActionResult(ActionStatus.Error);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var decodToken = String.Join("/", Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Decode(token).Select(_ => _.ToString()));
            smtp.SendResetPasswordRequest(email, decodToken);
            return new ActionResult(ActionStatus.Success);
        }

        public async Task<ActionResult> ResetPassword(string email, string password, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ActionResult(ActionStatus.Error);
            }

            var encodeToken = Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Encode(token.Split('/').Select(_ => byte.Parse(_)).ToArray()).Replace('_', '/').Replace('-', '+');
            var result = await _userManager.ResetPasswordAsync(user, encodeToken, password);
            if (result.Succeeded)
            {
                return new ActionResult(ActionStatus.Success, result);
            }
            else
            {
                return new ActionResult(ActionStatus.Error, result);
            }
        }

        public async Task<UserProfile> GetUserCredentilas(ClaimsPrincipal user) => new UserProfile(await _userManager.GetUserAsync(user));

        public AppUser GetUser(string id) => dbContext.Users.FirstOrDefault(_ => _.Id == id);

        public bool UserIsAuthorized() => _authorize.OnAuthorization();

        public async Task<ActionResult> UpdateUser(ClaimsPrincipal claimsPrincipal, UserProfile userProfile)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;
            user.PhoneNumber = userProfile.Phone;

            if (userProfile.Password != null && userProfile.Password != "")
            {
                var _passwordValidator = context.HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUser>)) as IPasswordValidator<AppUser>;
                var _passwordHasher = context.HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUser>)) as IPasswordHasher<AppUser>;

                IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, user, userProfile.Password);

                if (result.Succeeded)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, userProfile.Password);
                    await _userManager.UpdateAsync(user);
                    return new ActionResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionResult(ActionStatus.Error, result);
                }
            }

            await _userManager.UpdateAsync(user);
            return new ActionResult(ActionStatus.Success);
        }
    }
}
