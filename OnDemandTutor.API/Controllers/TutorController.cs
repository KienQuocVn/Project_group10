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

        [HttpGet("{id}")]
        public async Task<ActionResult<TutorSubject>> GetTutorSubjectById(Guid id)
        {
            var tutorSubject = await _tutorService.GetTutorSubjectByIdAsync(id);
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
                var createdTutorSubject = await _tutorService.CreateTutorSubjectAsync(model);
                return CreatedAtAction(nameof(GetTutorSubjectById), new { id = createdTutorSubject.Id }, createdTutorSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutorSubject(Guid id, [FromBody] UpdateTutorSubjectModelViews model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Model cannot be null" });
            }

            var existingTutorSubject = await _tutorService.GetTutorSubjectByIdAsync(id);
            if (existingTutorSubject == null)
            {
                return NotFound(new { Message = "Tutor subject not found" });
            }

            try
            {
                await _tutorService.UpdateTutorSubjectAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTutorSubject(Guid id)
        {
            var existingTutorSubject = await _tutorService.GetTutorSubjectByIdAsync(id);
            if (existingTutorSubject == null)
            {
                return NotFound(new { Message = "Tutor subject not found" });
            }

            try
            {
                bool isDeleted = await _tutorService.DeleteTutorSubjectAsync(id);
                if (!isDeleted) return BadRequest(new { Message = "Delete action failed" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}