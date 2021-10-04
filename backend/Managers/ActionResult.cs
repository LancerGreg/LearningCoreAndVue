using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers
{
    public class ActionAuthResult
    {
        public readonly ActionStatus _actionStatus;
        public readonly IdentityResult _identityResult = null;
        public readonly SignInResult _signInResult = null;

        public ActionAuthResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }

        public ActionAuthResult(ActionStatus actionStatus, IdentityResult identityResult)
        {
            _actionStatus = actionStatus;
            _identityResult = identityResult;
        }

        public ActionAuthResult(ActionStatus actionStatus, SignInResult signInResult)
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

    public class ActionAccountResult
    {
        public readonly ActionStatus _actionStatus;
        public readonly IdentityResult _identityResult = null;
        public string message = "";

        public ActionAccountResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }

        public ActionAccountResult(ActionStatus actionStatus, string message)
        {
            _actionStatus = actionStatus;
            this.message = message;
        }

        public ActionAccountResult(ActionStatus actionStatus, IdentityResult identityResult)
        {
            _actionStatus = actionStatus;
            _identityResult = identityResult;
        }

        public ActionAccountResult(ActionStatus actionStatus, IdentityResult identityResult, string message)
        {
            _actionStatus = actionStatus;
            _identityResult = identityResult;
            this.message = message;
        }

        public List<Error> GetErrorList(IdentityResult identityResult) =>
            identityResult.Errors.Select(_ => new Error() { Code = _.Code, Description = _.Description }).ToList();
    }

    public class ActionInvitedResult
    {
        public readonly ActionStatus _actionStatus;

        public readonly string actionMessage;

        public ActionInvitedResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }

        public ActionInvitedResult(ActionStatus actionStatus, string actionMessage)
        {
            _actionStatus = actionStatus;
            this.actionMessage = actionMessage;
        }
    }

    public class ActionFriendResult
    {
        public readonly ActionStatus _actionStatus;

        public readonly string actionMessage;

        public ActionFriendResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }

        public ActionFriendResult(ActionStatus actionStatus, string actionMessage)
        {
            _actionStatus = actionStatus;
            this.actionMessage = actionMessage;
        }
    }

    public class ActionInviteResult
    {
        public readonly ActionStatus _actionStatus;

        public readonly string actionMessage;

        public ActionInviteResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }

        public ActionInviteResult(ActionStatus actionStatus, string actionMessage)
        {
            _actionStatus = actionStatus;
            this.actionMessage = actionMessage;
        }
    }

    public class TestingResult
    {
        public readonly ActionStatus _actionStatus;

        public TestingResult(ActionStatus actionStatus)
        {
            _actionStatus = actionStatus;
        }
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
