using DiplomskaNaloga.Models;
using DiplomskaNaloga.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomskaNaloga.Controllers
{
    [Route("api/[controller]/[action]"), ApiController, Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignUp(UserRequest request)
        {
            try
            {
                await _authenticationService.SignUp(request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignIn(UserLogin request)
        {
            try
            {
                var result = await _authenticationService.SignIn(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

    }
}
