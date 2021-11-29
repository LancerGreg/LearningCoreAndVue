using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/friend")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet("get_my_friends")]
        public async Task<JsonResult> GetMyFriends() =>
            new JsonResult(await _friendService.GetFriends(User));

        [HttpGet("get_user_friends")]
        public JsonResult GetUserFriendsById(string userId) =>
            new JsonResult(_friendService.GetUserFriends(userId));

        [HttpGet("get_user_by_id")]
        public JsonResult GetUserById(string userId) =>
            new JsonResult(_friendService.GetUserById(userId));

        [HttpGet("get_user_by_email")]
        public async Task<JsonResult> GetUserByEmail(string userEmail) =>
            new JsonResult(await _friendService.GetUserByEmail(User, userEmail.ToLower()));

        [HttpGet("get_user_by_phone")]
        public async Task<JsonResult> GetUserByPhone(string userPhone) =>
            new JsonResult(await _friendService.GetUserByPhone(User, userPhone));

        [HttpGet("get_user_by_name")]
        public async Task<JsonResult> GetUserByName(string firstName = "", string lastName = "") =>
            new JsonResult(await _friendService.GetUsersByName(User, firstName.ToLower(), lastName == null ? "" : lastName.ToLower()));

        [HttpGet("get_graph_data")]
        public async Task<JsonResult> GetGraphData(int range, bool simplifiedLink) =>
            new JsonResult(await _friendService.GetGraphData(User, range, simplifiedLink));

        [HttpGet("get_graph_data_friend")]
        public async Task<JsonResult> GetGraphData(int range, bool simplifiedLink, string friendId) =>
            new JsonResult(await _friendService.GetGraphData(User, range, simplifiedLink, friendId));
    }
}
