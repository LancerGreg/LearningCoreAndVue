using backend.Managers;
using backend.Models;
using Microsoft.AspNetCore.Identity;
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
        Task<ActionAccountResult> UpdateUser(ClaimsPrincipal claimsPrincipal, UserProfile userProfile);
    }
}
