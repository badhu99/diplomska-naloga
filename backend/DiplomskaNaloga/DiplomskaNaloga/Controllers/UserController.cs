using System.Net;
using DiplomskaNaloga.Models;
using DiplomskaNaloga.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomskaNaloga.Controllers
{
    [Route("api/[controller]/[action]"), ApiController, Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, ProducesResponseType(typeof(List<UserDto>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("{userId}"), AllowAnonymous]
        public async Task<IActionResult> Activate(Guid userId, UserActivate request)
        {
            try
            {
                await _userService.ChangeUserActive(userId, request.IsActive);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
