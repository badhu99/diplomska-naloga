using DiplomskaNaloga.Models;
using DiplomskaNaloga.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomskaNaloga.Controllers
{
    [Route("api/[controller]/[action]"), ApiController, Authorize]
	public class SensorDataController:BaseController
	{
		private readonly ISensorDataService _service;
		private readonly ILogger<SensorDataController> _logger;

        public SensorDataController(ISensorDataService service, ILogger<SensorDataController> logger)
		{
            _service = service;
            _logger = logger;
        }

		[HttpPost("{id}"), AllowAnonymous]
		public async Task<IActionResult> Add([FromBody] SensorDetailsData body, Guid id) {
			try
			{
                string apiKey = Request.Headers["X-API-Key"];

				var allowed = await _service.VerifyApiKey(id, apiKey);
				if (allowed == false) return Unauthorized();

                await _service.AddData(id, body);

				return NoContent();
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			
		}

		[HttpGet("{sensorGroupId}"), AllowAnonymous]
		public async Task<IActionResult> GetData(Guid sensorGroupId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null) {
			try
			{
                var result = await _service.GetData(UserId, sensorGroupId, startDate, endDate);
                return Ok(result);
			}
			catch (UnauthorizedAccessException e) {
				return Unauthorized();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
	}
}

