using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public interface ISMTP
    {
        void SendSignUpRequest(string toEmail, string TokenVerified);
    }
}
