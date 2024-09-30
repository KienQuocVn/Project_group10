using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews;
using OnDemandTutor.Services.Service;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService) => _subjectService = subjectService;

        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<Subject>>> GetAllSubjects(int pageNumber, int pageSize)
        {
            try
            {
                var subjects = await _subjectService.GetAllSubject(pageNumber, pageSize);
                return Ok(BaseResponse<BasePaginatedList<Subject>>.OkResponse(subjects));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // Tr? v? thông báo l?i  
            }
        }

        // Ph??ng th?c POST ?? thêm m?i Subject  
        [HttpPost("new_subject")]
        public async Task<IActionResult> AddSubject([FromBody] CreateSubjectModelViews model)
        {
            try
            {
                var subject = await _subjectService.CreateSubjectAsync(model);
                return Ok(BaseResponse<Subject>.OkResponse(subject)); // Tr? v? Subject ?ã t?o  
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // Tr? v? thông báo l?i  
            }
        }

        // Ph??ng th?c c?p nh?t môn h?c  
        [HttpPut("update_subject")]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectModel model)
        {
            // G?i d?ch v? ?? c?p nh?t môn h?c  
            var updatedSubject = await _subjectService.UpdateSubject(model);

            // Ki?m tra k?t qu? c?p nh?t  
            if (updatedSubject == null)
            {
                return BadRequest(new { Message = "Không tìm th?y môn h?c v?i ID: " + model.Id });
            }

            // Tr? v? thông báo thành công  
            return Ok(new { Message = "C?p nh?t môn h?c thành công", Subject = updatedSubject });
        }

        // Ph??ng th?c xóa môn h?c  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            // G?i d?ch v? ?? xóa môn h?c  
            var result = await _subjectService.DeleteSubject(id);

            // Ki?m tra k?t qu? xóa  
            if (!result)
            {
                return BadRequest(new { Message = "Không tìm th?y môn h?c v?i ID: " + id });
            }

            // Tr? v? mã 204 No Content n?u xóa thành công  
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSubjectsByName(string subjectName, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _subjectService.SearchSubjectsByNameAsync(subjectName, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có l?i x?y ra khi tìm ki?m môn h?c.");
            }
        }
    }
}
