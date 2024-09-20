using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.UserModelViews;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        public ClassController() {}
        private readonly IClassService _classService;
        [HttpGet("get_all_classes")]
        public async Task<IActionResult> getAllClass()
        {
            var classes = await _classService.GetAllClass();
            return Ok(BaseResponse<IList<Class>>.OkResponse(classes));
        }

        [HttpPost("add_class")]
        public async Task<IActionResult> AddClass()
        {
            return Ok();
        }
    }
}
