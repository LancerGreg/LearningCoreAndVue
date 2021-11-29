namespace backend.Managers.ActionResult.Responses
{
    public class AuthResponse : BaseResponse
    {
        public static Response UserNotFound() => new Response() 
        { 
            Code = nameof(UserNotFound), 
            StatusCode = 404, 
            Description = "User is not found" 
        };

        public static Response NotImplemented() => new Response() 
        { 
            Code = nameof(NotImplemented), 
            StatusCode = 501, 
            Description = "The server does not support the functionality required to process the request" 
        };

        public static Response BadRequest() => new Response() 
        { 
            Code = nameof(BadRequest), 
            StatusCode = 400, 
            Description = "The request is not understood by the server due to an incorrect syntax" 
        };

        public static Response NotCorrectPasswrod() => new Response() 
        { 
            Code = nameof(NotCorrectPasswrod), 
            StatusCode = 400, 
            Description = "Your password and confirmation password do not match" 
        };
    }

    public class SignInResponse : AuthResponse
    {
        public static Response IsLockedOut() => new Response()
        { 
            Code = nameof(IsLockedOut), 
            StatusCode = 423, 
            Description = "The user is locked out" 
        };

        public static Response IsNotAllowed() => new Response() 
        {
            Code = nameof(IsNotAllowed), 
            StatusCode = 405, 
            Description = "The user is not allowed" 
        };

        public static Response RequiresTwoFactor() => new Response() 
        { 
            Code = nameof(RequiresTwoFactor),
            StatusCode = 400, 
            Description = "The user requires two factor authentication" 
        };

        public static Response FailUserCredentials() => new Response() 
        { 
            Code = nameof(FailUserCredentials), 
            StatusCode = 401, 
            Description = "Incorrect username or password" 
        };
    }
}
