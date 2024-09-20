using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.FeedbackModelViews;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private IFeedbackService _feedbackSevice;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackSevice = feedbackService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedbackById(string id)
        {
            var feedback = await _feedbackSevice.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        [HttpPost()]
        public async Task<ActionResult<Feedback>> CreateFeedback(CreateFeedbackModelViews model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Model cannot be null" });
            }

            try
            {
                var createdFeedback = await _feedbackSevice.CreateFeedbackAsync(model);
                return CreatedAtAction(nameof(GetFeedbackById), new { id = createdFeedback.Id }, createdFeedback);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<Feedback>> GetAllFeedback(int pageNumber, int pageSize)
        {
            try
            {
                var Sub = await _feedbackSevice.GetAllFeedbackAsync(pageNumber, pageSize);
                return Ok(BaseResponse<BasePaginatedList<Feedback>>.OkResponse(Sub));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateFeedback(string id, Guid studentId, UpdateFeedbackModelViews model)
        {
            if (model == null)
            {
                return BadRequest(new { Message = "Model cannot be null" });
            }

            try
            {
                var result = await _feedbackSevice.UpdateFeedbackAsync(id, studentId, model);

                if (result)
                {
                    return Ok(new { message = "Feedback updated successfully." });
                }

                return BadRequest(new { message = "Failed to update feedback." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating feedback.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFeedback(string id, Guid studentId)
        {

            try
            {
                var result = await _feedbackSevice.DeleteFeedbackAsync(id, studentId);

                if (result)
                {
                    return Ok(new { message = "Feedback delete successfully." });
                }

                return BadRequest(new { message = "Failed to delete feedback." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while delete feedback.", error = ex.Message });
            }
        }
    }
}
