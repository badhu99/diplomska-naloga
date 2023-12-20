using DiplomskaNaloga.Models;
using DiplomskaNaloga.Services;
using DipslomskaNaloga.Utility.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomskaNaloga.Controllers
{
    [Route("api/[controller]/[action]"), ApiController, Authorize]
    public class SensorsController : BaseController
    {
        private readonly ISensorService _sensorService;

        public SensorsController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSensors(int pageNumber = 1, int pageSize = 12, bool orderDesc = false, EnumSensorGroup orderBy = EnumSensorGroup.Name)
        {
            try
            {
                var result = await _sensorService.GetPagination(UserId, pageNumber, pageSize, orderDesc, orderBy);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSensorGroup(SensorGroupData data)
        {
            try
            {
                var c = Role;
                var result = await _sensorService.AddNewSensorGroup(UserId!.Value, data);
                return Created(result.ToString(), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensorGroup(Guid id)
        {
            try
            {
                await _sensorService.DeleteSensorGroup(UserId!.Value, id, Role);
                return NoContent();
            }
            catch (Exception _)
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSensorGroup(Guid id, SensorGroupData data)
        {
            try
            {
                await _sensorService.UpdateSensorGroup(UserId!.Value, id, data, Role);
                return NoContent();
            }
            catch (Exception _)
            {
                return BadRequest(_.Message);
            }
        }
    }
}
