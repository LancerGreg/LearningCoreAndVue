using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Managers.ActionResult
{
    public class ActionTestingResult : ActionResult
    {
        public ActionTestingResult(ActionStatus actionStatus, Response response) : base(actionStatus, response)
        {

        }
    }
}
