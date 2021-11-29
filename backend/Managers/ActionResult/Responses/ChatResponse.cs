using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult.Responses
{
    public class ChatResponse : BaseResponse
    {
        public static Response Forbidden() => new Response() { Code = nameof(Forbidden), StatusCode = 403, Description = "Access denied due to unauthorized access" };
        public static Response EmptyValue() => new Response() { Code = nameof(EmptyValue), StatusCode = 400, Description = "A new name must be specified" };
        public static Response LongValue() => new Response() { Code = nameof(LongValue), StatusCode = 400, Description = "A new name must be smaller than 64 characters" };
    }
}
