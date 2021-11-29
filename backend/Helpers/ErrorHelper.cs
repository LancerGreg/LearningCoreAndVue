using backend.Helpers.Interfaces;
using backend.Managers.ActionResult;
using backend.Managers.ActionResult.Responses;
using Microsoft.AspNetCore.Mvc;

namespace backend.Helpers
{
    public class ErrorHelper : IErrorHelper
    {
        public IActionResult GetInternalError() =>
             new ActionErrorResult(ActionStatus.Error, BaseResponse.InternalError()).GetActionResult();
        public IActionResult GetPageNotFound() =>
             new ActionErrorResult(ActionStatus.Error, BaseResponse.PageNotFound()).GetActionResult();
    }
}
