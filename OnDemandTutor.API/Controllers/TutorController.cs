using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
namespace OnDemandTutor.API.Controllers
{
    // Định nghĩa route cho controller này  
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService _tutorService;

        // Khởi tạo controller với dịch vụ gia sư  
        public TutorController(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        // Phương thức GET để lấy tất cả các môn học của gia sư với phân trang  
        // Phương thức GET để lấy tất cả các môn học của gia sư với phân trang  
        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<TutorSubject>>> GetAllTutorSubjects(int pageNumber = 1, int pageSize = 5)
        {
            // Gọi dịch vụ để lấy danh sách môn học của gia sư  
            var result = await _tutorService.GetAllTutorSubjectsAsync(pageNumber, pageSize);

            // Kiểm tra nếu result là null (có thể do lỗi trong service)  
            if (result == null)
            {
                return StatusCode(500, new { Message = "Internal server error" });
            }

            // Trả về danh sách môn học của gia sư  
            return Ok(result);
        }

        // Phương thức GET để lấy thông tin môn học của gia sư theo ID  
        [HttpGet("id/{tutorId}/{subjectId}")]
        public async Task<ActionResult<TutorSubject>> GetByTutorIdSubjectId(Guid tutorId, string subjectId)
        {
            // Gọi dịch vụ để lấy môn học của gia sư theo ID  
            var tutorSubject = await _tutorService.GetByTutorIdSubjectIdAsync(tutorId, subjectId);

            // Kiểm tra nếu tutorSubject là null (có thể do lỗi trong service)  
            if (tutorSubject == null)
            {
                return NotFound(new { Message = "Tutor subject not found" });
            }

            // Nếu không có lỗi, trả về kết quả 200 OK  
            return Ok(tutorSubject);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTutorSubjectsByName(string subjectName, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _tutorService.SearchTutorSubjectsByNameAsync(subjectName, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có lỗi xảy ra khi tìm kiếm môn học.");
            }
        }

        // Phương thức POST để tạo môn học mới cho gia sư  
        [HttpPost]
        public async Task<ActionResult<TutorSubject>> CreateTutorSubject([FromBody] CreateTutorSubjectModelViews model)
        {
            // Kiểm tra xem model có null hay không  
            if (model == null)
            {
                return BadRequest(new { Message = "Model cannot be null" });
            }

            try
            {
                // Thiết lập thông tin tạo mới  
                model.CreatedBy = "admin";
                model.CreatedTime = DateTime.UtcNow;

                // Gọi dịch vụ để tạo môn học mới  
                var createdTutorSubject = await _tutorService.CreateTutorSubjectAsync(model);

                // Trả về kết quả 201 Created với thông tin môn học vừa tạo  
                return CreatedAtAction(nameof(CreateTutorSubject), new { id = createdTutorSubject.Id }, createdTutorSubject);
            }
            catch (Exception ex)
            {
                // Trả về lỗi mà không ghi log  
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        // Phương thức PUT để cập nhật thông tin môn học của gia sư  
        [HttpPut("{tutorId}/{subjectId}")]
        public async Task<IActionResult> UpdateTutorSubject(Guid tutorId, string subjectId, [FromBody] UpdateTutorSubjectModelViews model)
        {
            // Kiểm tra xem model có null hay không  
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Gọi dịch vụ để cập nhật môn học  
                var updatedTutorSubject = await _tutorService.UpdateTutorSubjectAsync(tutorId, subjectId, model);

                // Kiểm tra nếu không tìm thấy môn học để cập nhật  
                if (updatedTutorSubject == null)
                {
                    return NotFound("TutorSubject not found.");
                }

                // Trả về kết quả 200 OK với thông tin môn học đã cập nhật  
                return Ok(updatedTutorSubject);
            }
            catch (Exception ex)
            {
                // Trả về lỗi mà không ghi log  
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        // Phương thức DELETE để xóa môn học của gia sư theo ID  
        [HttpDelete("deleteByTutorIdAndSubjectId")]
        public async Task<IActionResult> DeleteByTutorIdAndSubjectId(Guid tutorId, string subjectId)
        {
            try
            {
                // Gọi dịch vụ để xóa môn học  
                bool isDeleted = await _tutorService.DeleteTutorSubjectByTutorIdAndSubjectIdAsync(tutorId, subjectId);

                // Kiểm tra nếu không tìm thấy môn học để xóa  
                if (!isDeleted)
                {
                    return NotFound(new { Message = "Tutor subject not found" });
                }
                return NoContent(); // Trả về 204 No Content nếu xóa thành công  
            }
            catch (Exception ex)
            {
                // Trả về lỗi mà không ghi log  
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}