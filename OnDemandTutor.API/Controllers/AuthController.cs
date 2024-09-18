using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.ModelViews.AuthModelViews;
using OnDemandTutor.Contract.Services.Interface;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // Inject the user service via constructor
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Authenticate user
            var account = await _userService.AuthenticateAsync(model);
            if (account == null)
                return Unauthorized("Invalid credentials");

            // Add login logic here (optional token generation, etc.)
            return Ok(account); // Return user information or JWT token
        }

        // Register API
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateAccountModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create new account
            var account = await _userService.CreateAccountAsync(model);
            if (account == null)
                return BadRequest("Failed to create account");

            // You may want to return some details about the new account
            return Ok(account);
        }
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string roleName, string createdBy)
        {
            var result = await _userService.AddRoleAsync(roleName, createdBy);
            if (!result)
            {
                return BadRequest("Role already exists or creation failed.");
            }

            return Ok("Role created successfully.");
        }

        [HttpPost("add-role-to-user")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleModel model )
        {
            var result = await _userService.AddRoleToAccountAsync(model.UserId, model.RoleName);
            if (!result)
            {
                return BadRequest("Failed to add role to user or user/role does not exist.");
            }

            return Ok("Role added to user successfully.");
        }

        [HttpPost("add-claim-to-role")]
        public async Task<IActionResult> AddClaimToRole([FromBody] AddClaimToRoleModel model)
        {
            var result = await _userService.AddClaimToRoleAsync(model.RoleId, model.ClaimType, model.ClaimValue, model.CreatedBy);
            if (!result)
            {
                return BadRequest("Failed to add claim to role or role does not exist.");
            }

            return Ok("Claim added to role successfully.");
        }
        [HttpPost("add-claim")]
        public async Task<IActionResult> AddClaimToUser([FromBody] AddClaimModel model)
        {
            var result = await _userService.AddClaimToUserAsync(model.UserId, model.ClaimType, model.ClaimValue, model.CreatedBy);
            if (!result)
            {
                return BadRequest("Failed to add claim.");
            }

            return Ok("Claim added successfully.");
        }

        // API để lấy danh sách các claim của người dùng
        [HttpGet("user-claims/{userId}")]
        public async Task<IActionResult> GetUserClaims(Guid userId)
        {
            var claims = await _userService.GetUserClaimsAsync(userId);
            return Ok(claims);
        }

        // API để cập nhật claim
        [HttpPut("update-claim")]
        public async Task<IActionResult> UpdateClaim([FromBody] UpdateClaimModel model)
        {
            var result = await _userService.UpdateClaimAsync(model.ClaimId, model.ClaimType, model.ClaimValue, model.UpdatedBy);
            if (!result)
            {
                return BadRequest("Failed to update claim.");
            }

            return Ok("Claim updated successfully.");
        }

        // API để xóa mềm claim
        [HttpDelete("delete-claim/{claimId}")]
        public async Task<IActionResult> SoftDeleteClaim(Guid claimId, [FromBody] string deletedBy)
        {
            var result = await _userService.SoftDeleteClaimAsync(claimId, deletedBy);
            if (!result)
            {
                return BadRequest("Failed to delete claim.");
            }

            return Ok("Claim deleted successfully.");
        }

    }
}
