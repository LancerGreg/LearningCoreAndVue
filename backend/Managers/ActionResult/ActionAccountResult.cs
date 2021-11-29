using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Mvc;

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
