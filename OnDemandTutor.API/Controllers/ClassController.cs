using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ClassModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        // GET: api/Class
        [HttpGet()]
        public async Task<ActionResult<BasePaginatedList<Class>>> GetAllClasses(int pageNumber = 1,int pageSize = 5, Guid? accountId = null,string? subjectId = null,DateTime? startDay = null,DateTime? endDay = null)
        {
            try
            {
                // Gọi service với các tham số tìm kiếm
                var result = await _classService.GetAllClassesAsync(pageNumber, pageSize, accountId, subjectId, startDay, endDay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClassById(string id)
        {
            try
            {
                // Gọi service để lấy class theo ID
                var result = await _classService.GetClassByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }



        // POST: api/Class
        [HttpPost()]
        public async Task<ActionResult<ResponseClassModelView>> CreateClass([FromBody] CreateClassModelView model)
        {
            try
            {
                ResponseClassModelView result = await _classService.CreateClassAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Class/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(string id, [FromBody] UpdateClassModelView model)
        {
            try
            {
                ResponseClassModelView result = await _classService.UpdateClassAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Class/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(string id)
        {
            try
            {
                ResponseClassModelView result = await _classService.DeleteClassAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Class
        [HttpGet("GetTotalAmountClasses/{id}")]
        public async Task<IActionResult> GetTotalAmountClasses(string id)
        {
            try
            {
                double totalAmount = await _classService.CalculateTotalAmount(id);
                return Ok(totalAmount);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
