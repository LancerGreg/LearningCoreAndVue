using backend.Managers.ActionResult.Responses;

namespace backend.Managers.ActionResult
{
    public class ActionTestingResult : ActionResult
    {
        public ActionTestingResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }
    }
}
