using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers
{
    public class ActionResult
    {
        public readonly ActionStatus _actionStatus;
        public readonly IdentityResult _identityResult = null;
        public readonly SignInResult _signInResult = null;

        public ActionResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }

        public ActionResult(ActionStatus actionStatus, IdentityResult identityResult)
        {
            _actionStatus = actionStatus;
            _identityResult = identityResult;
        }

        public ActionResult(ActionStatus actionStatus, SignInResult signInResult)
        {
            _actionStatus = actionStatus;
            _signInResult = signInResult;
        }

        public List<Error> GetErrorList(SignInResult signInResult)
        {
            var SignInErrors = new List<Error>();

            if (signInResult.IsLockedOut)
                SignInErrors.Add(new Error() { Code = "IsLockedOut", Description = "The user is locked out" });
            if (signInResult.IsNotAllowed)
                SignInErrors.Add(new Error() { Code = "IsNotAllowed", Description = "The user is not allowed" });
            if (signInResult.RequiresTwoFactor)
                SignInErrors.Add(new Error() { Code = "RequiresTwoFactor", Description = "The user requires two factor authentication" });
            if (!SignInErrors.Any())
                SignInErrors.Add(new Error() { Code = "FailUserCredentials", Description = "Incorrect username or password" });

            return SignInErrors;
        }

        public List<Error> GetErrorList(IdentityResult identityResult) =>
            identityResult.Errors.Select(_ => new Error() { Code = _.Code, Description = _.Description }).ToList();
    }

    public enum ActionStatus
    {
        Error = 0,
        Success = 1
    }

    public class Error {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
