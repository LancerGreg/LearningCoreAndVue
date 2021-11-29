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
using backend.Services.Interfaces;
using backend.Managers.ActionResult;
using Microsoft.AspNetCore.Mvc;
using backend.Managers.ActionResult.Responses;

namespace backend.Services
{
    public class AccountService : AppDbRepository, IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _context;
        private readonly IAuthService _authService;

        public AccountService(AppDbContext dbContext, UserManager<AppUser> userManager, IHttpContextAccessor context, IAuthService authService) : base(dbContext)
        {
            _userManager = userManager;
            _context = context;
            _authService = authService;
        }

        public AppUser GetUser(string id) => dbContext.Users.FirstOrDefault(_ => _.Id == id);

        public async Task<UserProfile> GetUserCredentilas(ClaimsPrincipal user) => new UserProfile(await _userManager.GetUserAsync(user));

        public async Task<IActionResult> UpdateUser(ClaimsPrincipal claimsPrincipal, UserProfile userProfile)
        {
            var message = "";
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;

            if (user.PhoneNumber != userProfile.Phone)
            {
                try
                {
                    await _authService.ResetNumberPhone(claimsPrincipal, userProfile.Phone);
                    message = "ResetNumberPhoneTokenSend";
                }
                catch (Exception e)
                {
                    return new ActionAuthResult(ActionStatus.Error, AccountResponse.UnsucceededPhone(e.Message)).GetActionResult();
                }               
            }
            user.PhoneNumber = userProfile.Phone;

            if (userProfile.Password != null && userProfile.Password != "")
            {
                var _passwordValidator = _context.HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUser>)) as IPasswordValidator<AppUser>;
                var _passwordHasher = _context.HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUser>)) as IPasswordHasher<AppUser>;

                IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, user, userProfile.Password);

                if (result.Succeeded)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, userProfile.Password);
                    await _userManager.UpdateAsync(user);
                    return new ActionAccountResult(ActionStatus.Success, AccountResponse.Success(message)).GetActionResult();
                }
                else
                {
                    return new ActionIdentityResult(ActionStatus.Error, IdentityResponse.IdentityError(result)).GetActionResult();
                }
            }

            await _userManager.UpdateAsync(user);
            return new ActionAuthResult(ActionStatus.Success, AccountResponse.Success(message)).GetActionResult();
        }
    }
}
