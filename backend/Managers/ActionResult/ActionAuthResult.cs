using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult
{
    public class ActionAuthResult : ActionResult
    {
        public ActionAuthResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }

        public override IActionResult GetActionResult()
        {
            return new ObjectResult(_response) { StatusCode = _response.StatusCode };
        }
    }

    public class ActionSignInResult : ActionAuthResult
    {
        private Microsoft.AspNetCore.Identity.SignInResult _signInResult { get; set; }

        public ActionSignInResult(ActionStatus actionStatus, Response response, Microsoft.AspNetCore.Identity.SignInResult signInResult) : base(actionStatus, response)
        {
            _signInResult = signInResult;
        }

        public override IActionResult GetActionResult()
        {
            _response = GetSignInResponse();
            return new ObjectResult(_response) { StatusCode = _response.StatusCode };
        }

        private Response GetSignInResponse()
        {
            if (_signInResult.IsLockedOut) return SignInResponse.IsLockedOut();
            else if (_signInResult.IsNotAllowed) return SignInResponse.IsNotAllowed();
            else if (_signInResult.RequiresTwoFactor) return SignInResponse.RequiresTwoFactor();
            else return SignInResponse.FailUserCredentials();
        }
    }

    public class ActionIdentityResult : ActionAuthResult
    {
        public ActionIdentityResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {
            
        }

        public override IActionResult GetActionResult()
        {
            return new ObjectResult(_response) { StatusCode = _response.StatusCode };
        }
    }
}
