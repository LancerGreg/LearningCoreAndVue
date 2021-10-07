using backend.Managers.ActionResult.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult
{
    public class ActionErrorResult : ActionResult
    {
        public ActionErrorResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }
    }
}
