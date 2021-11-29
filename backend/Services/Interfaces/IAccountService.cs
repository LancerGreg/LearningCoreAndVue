using backend.Models;
using Microsoft.AspNetCore.Mvc;
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
