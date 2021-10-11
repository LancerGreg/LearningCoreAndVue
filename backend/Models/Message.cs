using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public string SenderId { get; set; }
        public AppUser Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; }
    }

    public static class MessageChunk
    {
        public static int MessagesNumber = 20;
    }
}
