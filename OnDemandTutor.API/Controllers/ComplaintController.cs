using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;

        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // GET: api/complaint
        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<Complaint>>> GetAllComplaints(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var complaints = await _complaintService.GetAllComplaintsAsync(pageNumber, pageSize);
                return Ok(BaseResponse<BasePaginatedList<Complaint>>.OkResponse(complaints));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        // GET: api/complaint/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Complaint>> GetComplaintById(Guid id)
        {
            try
            {
                var complaint = await _complaintService.GetComplaintByIdAsync(id);
                if (complaint == null)
                {
                    return NotFound(new { Message = "Complaint not found" });
                }

                return Ok(BaseResponse<Complaint>.OkResponse(complaint));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        // POST: api/complaint
        [HttpPost]
        public async Task<ActionResult<Complaint>> CreateComplaint([FromBody] CreateComplaintModel model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Invalid complaint data" });
            }

            try
            {
                var createdComplaint = await _complaintService.CreateComplaintAsync(model);
                return CreatedAtAction(nameof(GetComplaintById), new { id = createdComplaint.Id }, createdComplaint);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        // PUT: api/complaint/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComplaint(Guid id, [FromBody] UpdateComplaintModel model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Invalid complaint data" });
            }

            try
            {
                var updated = await _complaintService.UpdateComplaintAsync(id, model);
                if (!updated)
                {
                    return NotFound(new { Message = "Complaint not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        // DELETE: api/complaint/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(Guid id)
        {
            try
            {
                var deleted = await _complaintService.DeleteComplaintAsync(id);
                if (!deleted)
                {
                    return NotFound(new { Message = "Complaint not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }
    }
}
