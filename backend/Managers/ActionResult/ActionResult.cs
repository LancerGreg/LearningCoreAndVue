using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult
{
    public class ActionResult
    {
        public readonly ActionStatus _actionStatus;
        public Response _response;

        public ActionResult(ActionStatus actionStatus, Response response)
        {
            _actionStatus = actionStatus;
            _response = response;
        }

        public virtual IActionResult GetActionResult()
        {
            return new ObjectResult(_response) { StatusCode = _response.StatusCode };
        }
    }
}
