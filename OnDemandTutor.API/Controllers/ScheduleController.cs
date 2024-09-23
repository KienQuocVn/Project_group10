using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;
using System;
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

        // GET: api/Schedule
        [HttpGet()]
        public async Task<ActionResult<BasePaginatedList<Schedule>>> GetAllSchedules(int pageNumber = 1, int pageSize = 5, Guid? studentId = null, string? slotId = null, string status = null)
        {
            try
            {
                var result = await _scheduleService.GetAllSchedulesAsync(pageNumber, pageSize, studentId, slotId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Schedule
        [HttpGet("searchSchedule")]
        public async Task<ActionResult<BasePaginatedList<Schedule>>> SearchSchedules(int pageNumber = 1, int pageSize = 5, Guid? studentId = null, string? slotId = null, string status = null)
        {
            try
            {
                var result = await _scheduleService.GetAllSchedulesAsync(pageNumber, pageSize, studentId, slotId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Schedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetScheduleById(string id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        // POST: api/Schedule
        [HttpPost()]
        public async Task<ActionResult<Schedule>> CreateSchedule([FromBody] CreateScheduleModelViews model)
        {

            try
            {
                ResponseScheduleModelViews result = await _scheduleService.CreateScheduleAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Schedule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpdateScheduleModelViews model)
        {
            try
            {
                ResponseScheduleModelViews result = await _scheduleService.UpdateScheduleAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Schedule/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            try
            {
                ResponseScheduleModelViews result = await _scheduleService.DeleteScheduleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
