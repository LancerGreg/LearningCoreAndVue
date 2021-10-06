using backend.Managers;
using backend.Managers.ActionResult;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface IAccountService
    {
        AppUser GetUser(string id);
        Task<UserProfile> GetUserCredentilas(ClaimsPrincipal user);
        Task<IActionResult> UpdateUser(ClaimsPrincipal claimsPrincipal, UserProfile userProfile);
    }
}
