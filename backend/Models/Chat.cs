using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
