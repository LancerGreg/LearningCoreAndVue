using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.ResultStatus
{
    public enum SignUpStatus
    {
        Error = 500,
        Success = 200,
        UserAlreadyExists = 409
    }
}
