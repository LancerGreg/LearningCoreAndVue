using System;

namespace backend.Models
{
    public class InviteTable
    {
        public Guid InviteId { get; set; }
        public string WhenSend { get; set; }

        public string SenderId { get; set; }
        public string FullName => FirstName == "" && LastName == "" ? "No Name" : $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public InviteTable() { }

        public InviteTable(AppUser user)
        {
            SenderId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
