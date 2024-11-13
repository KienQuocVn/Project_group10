using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.Account
{
    public class UpdateAccountModel : PageModel
    {
        [BindProperty]
        public UpdateUserViewModel User { get; set; }  // Model chứa thông tin cập nhật

        private readonly IUserService _userService;
        private readonly ILogger<UpdateAccountModel> _logger;

        public UpdateAccountModel(IUserService userService, ILogger<UpdateAccountModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            // Load thông tin người dùng hiện tại
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return NotFound("User not found.");
            }

            // Gán giá trị cho ViewModel
            User = new UpdateUserViewModel
            {
                Email = user.Email,
                Gender = user.Gender
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for update request.");
                return Page();
            }

            // Gọi service để cập nhật tài khoản
            var result = await _userService.UpdateUserAsync(userId, new UpdateUserModel
            {
                Email = User.Email,
                Gender = User.Gender
            });

            if (!result)
            {
                _logger.LogWarning("Failed to update account for user: {UserId}", userId);
                ModelState.AddModelError(string.Empty, "User not found or update failed.");
                return Page();
            }

            _logger.LogInformation("Account updated successfully for user: {UserId}", userId);

            return RedirectToPage("/Account/Details", new { userId = userId }); // Redirect đến trang chi tiết sau khi cập nhật thành công
        }
    }

    public class UpdateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}
