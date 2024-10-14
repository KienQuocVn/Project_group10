using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using OnDemandTutor.Services.Service;
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
        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<TutorSubject>>> GetAllTutor(int pageNumber = 1, int pageSize = 5, Guid? TutorId = null, Guid? SubjectId = null)
        {
            try
            {
                var result = await _tutorService.GetAllTutor(pageNumber, pageSize, TutorId, SubjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchById(int pageNumber = 1, int pageSize = 5, Guid? TutorId = null, Guid? SubjectId = null)
        {
            try
            {
                var result = await _tutorService.SearchById(pageNumber, pageSize, TutorId, SubjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TutorSubject>> CreateTutorSubject([FromBody] CreateTutorSubjectModelViews model)
        {
            try
            {
                ResponseTutorModelViews result = await _tutorService.CreateTutorSubjectAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{tutorId}/{subjectId}")]
        public async Task<ActionResult> UpdateTutorSubject(Guid tutorId, Guid subjectId, [FromBody] UpdateTutorSubjectModelViews model)
        {
            try
            {
                ResponseTutorModelViews result = await _tutorService.UpdateTutorSubjectAsync(tutorId, subjectId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Phương thức DELETE để xóa môn học của gia sư theo ID  
        [HttpDelete("delete/{tutorId}/{subjectId}")]
        public async Task<IActionResult> DeleteTutorSubjectAsync(Guid tutorId, Guid subjectId)
        {
            try
            {
                ResponseTutorModelViews result = await _tutorService.DeleteTutorSubjectAsync(tutorId, subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}