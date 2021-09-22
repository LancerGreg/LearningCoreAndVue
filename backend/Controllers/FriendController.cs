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

        [HttpGet("get_my_friends")]
        public async Task<JsonResult> GetMyFriends() =>
            new JsonResult(await _friendService.GetFriends(User));

        [HttpGet("get_user_friends")]
        public JsonResult GetUserFriendsById(string userId) =>
            new JsonResult(_friendService.GetUserFriends(userId));

        [HttpGet("get_user_by_id")]
        public JsonResult GetUserById(string userId) =>
            new JsonResult(_friendService.GetUserById(userId));

        [HttpGet("get_user_by_name")]
        public JsonResult GetUserByName(string firstName = "", string lastName = "") =>
            new JsonResult(_friendService.GetUsersByName(User, firstName.ToLower(), lastName == null ? "" : lastName.ToLower()));
    }
}
