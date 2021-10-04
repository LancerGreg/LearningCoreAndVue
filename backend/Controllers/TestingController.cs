using backend.Managers;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly ITestingService _testingService;

        public TestingController(ITestingService testingService)
        {
            _testingService = testingService;
        }

        [HttpPost("create_test_users")]
        public async Task<IActionResult> CreateTestUsers(int count) =>
            GetActionResult(await _testingService.CreateTestUsers(count));

        [HttpPost("create_test_friendships")]
        public IActionResult CreateTestFriendships(int from, int? countUsers) =>
            GetActionResult(_testingService.CreateTestFriendships(from, countUsers));

        private IActionResult GetActionResult(backend.Managers.TestingResult actionResult)
        {
            if (actionResult._actionStatus == ActionStatus.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
