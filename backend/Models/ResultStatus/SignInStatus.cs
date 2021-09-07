using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.ResultStatus
{
    public enum SignInStatus
    {
        Error = 500,
        Success = 200,
        Redirect = 302,
        InvalidLogin = 401,
    }
}
