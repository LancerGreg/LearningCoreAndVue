using backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Interfaces
{
    public interface ITestingService
    {
        Task<TestingResult> CreateTestUsers(int count);
        Task<TestingResult> CreateTestFriendships(int from, int? countUsers);
    }
}
