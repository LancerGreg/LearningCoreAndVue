namespace backend.Managers.ActionResult.Responses
{
    public class AccountResponse : BaseResponse
    {
        public static Response UnsucceededPhone(string message) => new Response() 
        { 
            Code = "Error", StatusCode = 500, 
            Description = "The specified phone number is not processed by the server", 
            Message = message 
        };
    }
}
