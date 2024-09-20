using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService _tutorService;

        public TutorController(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        [HttpGet]
        public async Task<ActionResult<BasePaginatedList<TutorSubject>>> GetAllTutorSubjects(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var result = await _tutorService.GetAllTutorSubjectsAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
        [HttpGet("{tutorId}/subject/{subjectId}")]
        public async Task<ActionResult<TutorSubject>> GetByTutorIdSubjectId(Guid tutorId, string subjectId)
        {
            var tutorSubject = await _tutorService.GetByTutorIdSubjectIdAsync(tutorId, subjectId);
            if (tutorSubject == null)
            {
                return NotFound(new { Message = "Tutor subject not found" });
            }
            return Ok(tutorSubject);
        }

        [HttpPost]
        public async Task<ActionResult<TutorSubject>> CreateTutorSubject([FromBody] CreateTutorSubjectModelViews model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Model cannot be null" });
            }

            try
            {
                model.CreatedBy = "admin";
                model.CreatedTime = DateTime.UtcNow;

                var createdTutorSubject = await _tutorService.CreateTutorSubjectAsync(model);
                return CreatedAtAction(nameof(CreateTutorSubject), new { id = createdTutorSubject.Id }, createdTutorSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpPut("{tutorId}/{subjectId}")]
        public async Task<IActionResult> UpdateTutorSubject(Guid tutorId, string subjectId, [FromBody] UpdateTutorSubjectModelViews model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            var updatedTutorSubject = await _tutorService.UpdateTutorSubjectAsync(tutorId, subjectId, model);
            if (updatedTutorSubject == null)
            {
                return NotFound("TutorSubject not found.");
            }

            return Ok(updatedTutorSubject);
        }

        [HttpDelete("deleteByTutorIdAndSubjectId")]
        public async Task<IActionResult> DeleteByTutorIdAndSubjectId(Guid tutorId, string subjectId)
        {
            try
            {
                bool isDeleted = await _tutorService.DeleteTutorSubjectByTutorIdAndSubjectIdAsync(tutorId, subjectId);
                if (!isDeleted)
                {
                    return NotFound(new { Message = "Tutor subject not found" });
                }
                return NoContent(); // Xóa thành công  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}