using backend.Managers.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        /// <summary>
        /// The <see cref="IHubContext"/> context
        /// </summary>
        private readonly IHubContext<ChatHub> _hubContext;
        public MessagesController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("[action]")]
        public IActionResult GetMessages()
        {
            List<Message> messages = new List<Message>();

            var result = new
            {
                messages
            };

            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task SendMessage([FromBody] Message message)
        {
            await _hubContext.Clients.All.SendAsync("RefreshEvent", Json(new
            {
                text = message.Text,
                date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
            }));
        }

        public class Message
        {
            public string Text { get; set; }

            public DateTime Date { get; set; }
        }
    }
}