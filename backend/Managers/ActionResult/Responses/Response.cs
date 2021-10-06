using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult.Responses
{
    public class Response
    {
        public string Code { get; set; }
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
    }

    public class BaseResponse
    {
        public static Response Error() => new Response() { Code = "Error", StatusCode = 400, Description = "The request has failed" };
        public static Response Success() => new Response() { Code = "Success", StatusCode = 200, Description = "The request has succeeded" };
        public static Response Success(string message) => new Response() { Code = "Success", StatusCode = 200, Description = "The request has succeeded", Message = message };
    }
}
