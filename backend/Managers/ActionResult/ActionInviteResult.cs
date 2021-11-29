using backend.Managers.ActionResult.Responses;

namespace backend.Managers.ActionResult
{
    public class ActionInviteResult : ActionResult
    {
        public ActionInviteResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }
    }
}
