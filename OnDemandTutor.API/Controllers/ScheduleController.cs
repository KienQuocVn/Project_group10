﻿using Microsoft.AspNetCore.Authorization;
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
        // lấy tất cả các lịch và lọc theo các tham số được truyền vào
        [HttpGet()]
        public async Task<ActionResult<BasePaginatedList<Schedule>>> GetAllSchedules(int pageNumber = 1, int pageSize = 5, string? id = null, Guid? studentId = null, string? slotId = null, string status = null)
        {
            try
            {
                var result = await _scheduleService.GetAllSchedulesAsync(pageNumber, pageSize, id, studentId, slotId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Schedule
        // lấy tất cả lịch trừ những lịch đã được xóa và lọc theo các tham số được truyền vào
        [HttpGet("searchSchedule")]
        public async Task<ActionResult<BasePaginatedList<Schedule>>> SearchSchedules(int pageNumber = 1, int pageSize = 5, string? id = null, Guid? studentId = null, string? slotId = null, string status = null)
        {
            try
            {
                var result = await _scheduleService.GetAllSchedulesAsync(pageNumber, pageSize, id, studentId, slotId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // POST: api/Schedule
        // tạo 1 lịch mới chuyền vào studentID, SlotId, Status
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

        // PUT: api/Schedule/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule( string id, [FromBody] UpdateScheduleModelViews model)
        {
            try
            {
                // Gọi service để cập nhật schedule theo StudentId và SlotId
                ResponseScheduleModelViews result = await _scheduleService.UpdateScheduleAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Schedule/delete/{id}
        // Xóa lịch truyền vào gồm student id và slotId
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule( string id)
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
