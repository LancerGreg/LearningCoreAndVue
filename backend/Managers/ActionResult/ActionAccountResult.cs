using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult
{
    public class ActionAccountResult : ActionResult
    {
        public ActionAccountResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }

        public override IActionResult GetActionResult()
        {
            return new ObjectResult(_response) { StatusCode = _response.StatusCode };
        }
    }
}
