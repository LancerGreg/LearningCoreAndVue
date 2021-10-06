using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult.Responses
{
    public class IdentityResponse : BaseResponse
    {
        public static Response IdentityError(IdentityResult identityResult) => new Response() { Code = identityResult.Errors.FirstOrDefault().Code, StatusCode = 400, Description = identityResult.Errors.FirstOrDefault().Description };
    }
}
