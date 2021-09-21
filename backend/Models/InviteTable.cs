using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class InviteTable
    {
        public Guid InviteId { get; set; }
        public string WhenSend { get; set;}

        public string SenderId { get; set; }
        public string FullName => FirstName == "" && LastName == "" ? "No Name" : FirstName + " " + LastName;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
