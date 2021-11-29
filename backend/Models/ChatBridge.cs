using System;

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
