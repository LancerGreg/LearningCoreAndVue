using backend.Models;
using backend.Models.ResultStatus;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IAccountService
    {
        Task<SignUpStatus> SignUp(bool isValid, SignUpUser modelUser);
        Task<SignInStatus> SignIn(bool isValid, SignInUser modelUser, bool returnUrl);
        Task<LogoutStatus> Logout();
    }
}
