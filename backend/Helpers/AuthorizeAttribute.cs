using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;

namespace backend.Helpers
{
    public class AuthorizeAttribute : IAuthorizeAttribute
    {
        private readonly IHttpContextAccessor context;

        public AuthorizeAttribute(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public bool OnAuthorization() =>
            context.HttpContext.Session.Keys.Contains("AppUser");

        public void Authorization(string email, string token) =>
            context.HttpContext.Session.Set("AppUser", new AuthorizationUser(email, token));

        public void LogoutAuthorization() =>
            context.HttpContext.Session.Remove("AppUser");
    }

    public class AuthorizationUser
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthorizationUser(string Email, string Token)
        {
            this.Email = Email;
            this.Token = Token;
        }
    }
}
