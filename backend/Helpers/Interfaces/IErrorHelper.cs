using Microsoft.AspNetCore.Mvc;

namespace backend.Helpers.Interfaces
{
    public interface IErrorHelper
    {
        IActionResult GetInternalError();
        IActionResult GetPageNotFound();
    }
}
