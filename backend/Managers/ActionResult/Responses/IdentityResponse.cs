using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace backend.Managers.ActionResult.Responses
{
    public class IdentityResponse : BaseResponse
    {
        public static Response IdentityError(IdentityResult identityResult) => new Response()
        { 
            Code = identityResult.Errors.FirstOrDefault().Code, 
            StatusCode = 400, 
            Description = identityResult.Errors.FirstOrDefault().Description 
        };
    }
}
