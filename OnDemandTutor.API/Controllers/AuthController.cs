using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.ModelViews.AuthModelViews;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController() { }

        [HttpGet("auth_account")]
        public async Task<IActionResult> Login(LoginModelView model)
        {
            return Ok();
        }

        [HttpPost("new_account")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }

    }
}
