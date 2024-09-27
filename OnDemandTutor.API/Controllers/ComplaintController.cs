using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [ApiController] // Đánh dấu lớp này là một controller API
    [Route("api/[controller]")] // Định nghĩa route cho controller
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService; // Khai báo dịch vụ khiếu nại

        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService; // Khởi tạo dịch vụ khiếu nại
        }

        // Lấy tất cả khiếu nại với phân trang
        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<Complaint>>> GetAllComplaints(int pageNumber = 1, int pageSize = 5, Guid? studentId = null, Guid? tutorId = null, string? status = null)
        {
            var result = await _complaintService.GetAllComplaintsAsync(pageNumber, pageSize, studentId, tutorId, status); // Gọi dịch vụ để lấy danh sách khiếu nại
            return Ok(result); // Trả về kết quả với mã trạng thái 200
        }

        // Lấy một khiếu nại theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseComplaintModel>> GetComplaintById(Guid id)
        {
            var result = await _complaintService.GetComplaintByIdAsync(id); // Gọi dịch vụ để lấy khiếu nại theo ID
            return Ok(result); // Trả về kết quả với mã trạng thái 200
        }

        // Tạo một khiếu nại mới
        [HttpPost]
        public async Task<ActionResult<ResponseComplaintModel>> CreateComplaint([FromBody] CreateComplaintModel model)
        {
            var result = await _complaintService.CreateComplaintAsync(model); // Gọi dịch vụ để tạo khiếu nại mới
            return CreatedAtAction(nameof(GetComplaintById), new { id = result.Id }, result); // Trả về kết quả với mã trạng thái 201
        }

        // Cập nhật một khiếu nại theo ID
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseComplaintModel>> UpdateComplaint(Guid id, [FromBody] UpdateComplaintModel model)
        {
            var result = await _complaintService.UpdateComplaintAsync(id, model); // Gọi dịch vụ để cập nhật khiếu nại
            return Ok(result); // Trả về kết quả với mã trạng thái 200
        }

        // Xóa một khiếu nại theo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseComplaintModel>> DeleteComplaint(Guid id)
        {
            var result = await _complaintService.DeleteComplaintAsync(id); // Gọi dịch vụ để xóa khiếu nại
            return Ok(result); // Trả về kết quả với mã trạng thái 200
        }
    }
}
