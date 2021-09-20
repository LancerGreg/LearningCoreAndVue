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

namespace backend.Services
{
    public class AccountService : AppDbRepository, IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor context;

        public AccountService(AppDbContext dbContext, UserManager<AppUser> userManager, IHttpContextAccessor context) : base(dbContext)
        {
            _userManager = userManager;
            this.context = context;
        }

        public AppUser GetUser(string id) => dbContext.Users.FirstOrDefault(_ => _.Id == id);

        public async Task<UserProfile> GetUserCredentilas(ClaimsPrincipal user) => new UserProfile(await _userManager.GetUserAsync(user));

        public async Task<ActionAccountResult> UpdateUser(ClaimsPrincipal claimsPrincipal, UserProfile userProfile)
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
                    return new ActionAccountResult(ActionStatus.Success, result);
                }
                else
                {
                    return new ActionAccountResult(ActionStatus.Error, result);
                }
            }

            await _userManager.UpdateAsync(user);
            return new ActionAccountResult(ActionStatus.Success);
        }
    }
}
