using System;

namespace backend.Models
{
    public class Friendship
    {
        public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public string FriendId { get; set; }
    }
}
