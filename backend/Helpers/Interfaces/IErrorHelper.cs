using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers.Interfaces
{
    public interface IErrorHelper
    {
        IActionResult GetInternalError();
        IActionResult GetPageNotFound();
    }
}
