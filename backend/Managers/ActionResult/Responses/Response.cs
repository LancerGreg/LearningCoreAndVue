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
        public static Response Error() => new Response() 
        { 
            Code = nameof(Error), 
            StatusCode = 400, 
            Description = "The request has failed" 
        };

        public static Response NotAuthenticated() => new Response() 
        { 
            Code = nameof(NotAuthenticated), 
            StatusCode = 401, 
            Description = "User is not authenticated" 
        };

        public static Response PageNotFound() => new Response() 
        { 
            Code = nameof(PageNotFound), 
            StatusCode = 404, 
            Description = "Page not found" 
        };

        public static Response InternalError() => new Response()
        { 
            Code = nameof(InternalError),
            StatusCode = 500, 
            Description = "An error occurred on the server. Please try again later or contact your administrator" 
        };

        public static Response Success() => new Response() 
        { 
            Code = nameof(Success), 
            StatusCode = 200,
            Description = "The request has succeeded" 
        };

        public static Response Success(string message) => new Response() 
        { 
            Code = nameof(Success), 
            StatusCode = 200, 
            Description = "The request has succeeded", 
            Message = message 
        };
    }
}
