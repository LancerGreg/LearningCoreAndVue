using backend.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IErrorHelper _errorHelper;

        public ErrorController(IErrorHelper errorHelper)
        {
            _errorHelper = errorHelper;
        }

        [HttpGet("get_internal_error")]
        public IActionResult GetInternalError() =>
            _errorHelper.GetInternalError();

        [HttpGet("get_page_not_found")]
        public IActionResult GetPageNotFound() =>
            _errorHelper.GetPageNotFound();
    }
}
