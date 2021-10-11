using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult
{
    public class ActionChatResult : ActionResult
    {

        public ActionChatResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }

        public IActionResult GetActionResult(object data)
        {
            return _actionStatus == ActionStatus.Success
                ? new JsonResult(data)
                : new ObjectResult(_response) { StatusCode = _response.StatusCode };
        }
    }
}
