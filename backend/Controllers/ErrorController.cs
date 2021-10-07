using backend.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
