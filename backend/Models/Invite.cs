using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Invite
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public DateTime? WhenSend { get; set; }
        public DateTime? WhenDecide { get; set; }
        public Decide Decide { get; set; }
    }
}
