using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET: api/Schedule/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<BasePaginatedList<Schedule>>> GetAllSchedules(int pageNumber = 1, int pageSize = 5)
        {
            
            try
            {
                BasePaginatedList<Schedule> result = await _scheduleService.GetAllSchedulesAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // GET: api/Schedule/5
        [HttpGet("GetScheduleById/{id}")]
        public async Task<ActionResult<Schedule>> GetScheduleById(String id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        // POST: api/Schedule
        [HttpPost("CreateSchedule")]
        public async Task<ActionResult<Schedule>> CreateSchedule([FromBody] Schedule schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }

            var createdSchedule = await _scheduleService.CreateScheduleAsync(schedule);
            return CreatedAtAction(nameof(GetScheduleById), new { id = createdSchedule.Id }, createdSchedule);
        }

        // PUT: api/Schedule/5
        [HttpPut("UpdateSchedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(String id, [FromBody] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return BadRequest();
            }

            var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                return NotFound();
            }

            await _scheduleService.UpdateScheduleAsync(schedule);
            return NoContent();
        }

        // DELETE: api/Schedule/5
        [HttpDelete("DeleteSchedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(String id)
        {
            var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                return NotFound();
            }

            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }
    }
}
