using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomskaNaloga.Controllers
{
    [Route("api/[controller]/[action]"), ApiController, Authorize]
    public class SensorsController : ControllerBase
    {
        public SensorsController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetSensors()
        {
            return Ok("working");
        }
    }
}
