using backend.Managers;
using backend.Managers.ActionResult;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface ITestingService
    {
        Task<IActionResult> CreateTestUsers(int count);
        Task<IActionResult> CreateTestFriendships(int from, int? countUsers);
    }
}
