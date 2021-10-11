using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class ChatBridge
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
