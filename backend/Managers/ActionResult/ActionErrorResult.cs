using backend.Managers.ActionResult.Responses;

namespace backend.Managers.ActionResult
{
    public class ActionErrorResult : ActionResult
    {
        public ActionErrorResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }
    }
}
