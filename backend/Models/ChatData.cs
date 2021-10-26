using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class ChatData
    {
        public Chat chat { get; set; }
        public LastMessage lastMessage { get; set; }
        public List<Message> listMessages { get; set; }
    }

    public class LastMessage
    {
        public string text { get; set; }
        public string dateSend { get; set; }
    }
}
