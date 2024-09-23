using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews;

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
                var Sub = await _subjectService.GetAllSubject(pageNumber, pageSize);
                return Ok(BaseResponse<BasePaginatedList<Subject>>.OkResponse(Sub));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost ("new_subject")]
        public async Task<IActionResult> AddSubject([FromBody] String nameSubject)
        {
            await _subjectService.AddSubject(nameSubject);
            return Ok(BaseResponse<String>.OkResponse("Added Subject"));
        }

        [HttpPut("update_subject")]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectModel model)
        {
            var sub = await _subjectService.UpdateSubject(model);
            if (sub == null) return BadRequest(new { Message = "Subject not found" });
            return Ok(BaseResponse<UpdateSubjectModel>.OkResponse("Updated Subject"));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var result = await _subjectService.DeleteSubject(id);

            if (!result)
            {
                return BadRequest(new { Message = "Subject not found" });
            }

            return NoContent(); // Return 204 No Content if the deletion was successful
        }
    }
}
