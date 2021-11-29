using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using backend.Repositories;
using backend.Helpers.Interfaces;

namespace backend.Helpers
{
    public class AuthorizeHelper : AppDbRepository, IAuthorizeHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;

        public AuthorizeHelper(AppDbContext dbContext, IConfiguration configuration, IHttpContextAccessor context) : base(dbContext)
        {
            _configuration = configuration;
            _context = context;
        }

        public bool OnAuthorization()
        {
            var contain = _context.HttpContext.Session.Keys.Contains("AppUser");
            if (contain)
            {
                var userContext = _context.HttpContext.Session.Get<AuthorizationUser>("AppUser");
                return !(userContext.Email == null || userContext.Email == "");
            }
            
            return contain;
        }

        public void Authorization(string email) =>
            _context.HttpContext.Session.Set("AppUser", new AuthorizationUser(email));

        public void LogoutAuthorization() =>
            _context.HttpContext.Session.Remove("AppUser");
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
