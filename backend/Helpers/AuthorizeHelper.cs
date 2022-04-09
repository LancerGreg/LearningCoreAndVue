using backend.Helpers.Interfaces;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace backend.Helpers
{
    public class AuthorizeHelper : AppDbRepository, IAuthorizeHelper
    {
        private readonly IHttpContextAccessor _context;

        public AuthorizeHelper(AppDbContext dbContext, IHttpContextAccessor context) : base(dbContext)
        {
            _context = context;
        }

        public bool OnAuthorization()
        {
            var contain = _context.HttpContext.Session.Keys.Contains(nameof(AppUser));
            var userContext = _context.HttpContext.Session.Get<AuthorizationUser>(nameof(AppUser));

            if (contain)
                return contain;
            else
                return userContext == null 
                    ? false 
                    : !(userContext.Email == null || userContext.Email == "");
        }

        public void Authorization(string email) =>
            _context.HttpContext.Session.Set(nameof(AppUser), new AuthorizationUser(email));

        public void LogoutAuthorization() =>
            _context.HttpContext.Session.Remove(nameof(AppUser));
    }

    public class AuthorizationUser
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthorizationUser(string Email)
        {
            this.Email = Email;
        }
    }
}
