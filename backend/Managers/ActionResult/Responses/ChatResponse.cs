using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult.Responses
{
    public class ChatResponse : BaseResponse
    {
        public static Response Forbidden() => new Response() { Code = "Forbidden", StatusCode = 403, Description = "Access denied due to unauthorized access" };
    }
}
