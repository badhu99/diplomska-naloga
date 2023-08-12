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

		[HttpPost("{id}"), Authorize]
		public async Task<IActionResult> Add([FromBody] SensorDetailsData body, Guid id) {
			try
			{
                await _service.AddData(id, UserId!.Value, body);
                return NoContent();
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			
		}

		[HttpGet("{sensorGroupId}"), Authorize]
		public async Task<IActionResult> GetData(Guid sensorGroupId, [FromQuery] int pageSize = 12, [FromQuery] int pageNumber = 1) {
			try
			{
				var result = await _service.GetData(sensorGroupId, UserId!.Value, pageNumber, pageSize);
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

