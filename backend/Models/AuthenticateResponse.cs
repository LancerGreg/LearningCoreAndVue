using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(AppUser user, string token)
        {
            Id = user.Id;
            FirstName = user.UserName;
            LastName = user.UserName;
            Username = user.DisplayName;
            Email = user.Email;
            Token = token;
        }
    }
}
