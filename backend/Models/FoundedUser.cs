using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class FoundedUser
    {
        public string UserId { get; set; }
        public string FullName => FirstName == "" && LastName == "" ? "No Name" : FirstName + " " + LastName;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsFriend { get; set; }
        public bool HaveInvite { get; set; }

        public FoundedUser() { }

        public FoundedUser(AppUser user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
