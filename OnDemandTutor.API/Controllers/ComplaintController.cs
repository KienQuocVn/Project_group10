using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;

        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // Lấy tất cả khiếu nại với phân trang
        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<Complaint>>> GetAllComplaints(int pageNumber = 1, int pageSize = 5, Guid? studentId = null, Guid? tutorId = null, string? status = null)
        {
            try
            {
                var result = await _complaintService.GetAllComplaintsAsync(pageNumber, pageSize, studentId, tutorId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Trả về mã lỗi 500 với thông báo cụ thể
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // Lấy một khiếu nại theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseComplaintModel>> GetComplaintById(Guid id)
        {
            try
            {
                var result = await _complaintService.GetComplaintByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Trả về mã lỗi 404 nếu không tìm thấy hoặc mã lỗi 500 cho các lỗi khác
                return ex.Message.Contains("not found") ? NotFound(new { Message = ex.Message }) : StatusCode(500, new { Message = ex.Message });
            }
        }

        // Tạo một khiếu nại mới
        [HttpPost]
        public async Task<ActionResult<ResponseComplaintModel>> CreateComplaint([FromBody] CreateComplaintModel model)
        {
            try
            {
                var result = await _complaintService.CreateComplaintAsync(model);
                return CreatedAtAction(nameof(GetComplaintById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                // Trả về mã lỗi 400 cho các lỗi liên quan đến dữ liệu
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Cập nhật một khiếu nại theo ID
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseComplaintModel>> UpdateComplaint(Guid id, [FromBody] UpdateComplaintModel model)
        {
            try
            {
                var result = await _complaintService.UpdateComplaintAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex.Message.Contains("not found") ? NotFound(new { Message = ex.Message }) : StatusCode(500, new { Message = ex.Message });
            }
        }

        // Xóa một khiếu nại theo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseComplaintModel>> DeleteComplaint(Guid id)
        {
            try
            {
                var result = await _complaintService.DeleteComplaintAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex.Message.Contains("not found") ? NotFound(new { Message = ex.Message }) : StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
