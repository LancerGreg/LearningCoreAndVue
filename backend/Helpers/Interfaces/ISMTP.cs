using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers.Interfaces
{
    public interface ISMTP
    {
        void SendSignUpRequest(string toEmail, string tokenVerified);
        void SendResetPasswordRequest(string toEmail, string tokenVerified);
    }
}
