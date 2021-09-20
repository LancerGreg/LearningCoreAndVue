using backend.Managers;
using backend.Models;
using backend.Services;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("get_friends")]
        public async Task<JsonResult> GetFriends() =>
            new JsonResult(await _friendService.GetFriends(User));

        [HttpGet("get_friends_by_id")]
        public JsonResult GetUserFriendsById(string friendId) =>
            new JsonResult(_friendService.GetFriends(friendId));
    }
}
