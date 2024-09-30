using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TestJwtController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]  // Thêm để mô tả mã trạng thái 200
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]  // Mô tả mã trạng thái 401
    public IActionResult TestAuth()
    {
        return Ok(new { message = "You are authorized!" });
    }

    [HttpGet("public")]
    [ProducesResponseType(StatusCodes.Status200OK)]  // Thêm để mô tả mã trạng thái 200
    public IActionResult PublicEndpoint()
    {
        return Ok(new { message = "This is a public endpoint." });
    }
}