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
    public class AuthorizeAttribute : AppDbRepository, IAuthorizeAttribute
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor context;

        public AuthorizeAttribute(AppDbContext dbContext, IConfiguration configuration, IHttpContextAccessor context) : base(dbContext)
        {
            _configuration = configuration;
            this.context = context;
        }

        public bool OnAuthorization()
        {
            var contain = context.HttpContext.Session.Keys.Contains("AppUser");
            if (contain)
            {
                var userContext = context.HttpContext.Session.Get<AuthorizationUser>("AppUser");
                return !(userContext.Email == null || userContext.Email == "");
            }
            else
                return contain;
        }

        public void Authorization(string email) =>
            context.HttpContext.Session.Set("AppUser", new AuthorizationUser(email));

        public void LogoutAuthorization() =>
            context.HttpContext.Session.Remove("AppUser");
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
