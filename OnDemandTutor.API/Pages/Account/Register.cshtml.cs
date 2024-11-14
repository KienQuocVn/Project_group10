using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.API.Models;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel User { get; set; }  // Model ch?a thông tin ??ng ký

        private readonly IUserService _userService;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public void OnGet()
        {
            // Kh?i t?o logic n?u c?n khi vào trang l?n ??u
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Ki?m tra tính h?p l? c?a Model
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for registration request.");
                return Page();
            }

            // T?o tài kho?n m?i
            var account = await _userService.CreateAccountAsync(new CreateAccountModel
            {
                Email = User.Email,
                Password = User.Password,
                Gender = User.Gender
            });

            if (account == null)
            {
                _logger.LogWarning("Failed to create account for email: {Email}", User.Email);
                ModelState.AddModelError(string.Empty, "Failed to create account");
                return Page();
            }

            if (User.Gender != "Male" && User.Gender != "Female")
            {
                ModelState.AddModelError("User.Gender", "Gender not correct");
                return Page();
            }

            _logger.LogInformation("Account created successfully for email: {Email}", User.Email);
            return RedirectToPage("/Account/Login");  // Chuy?n h??ng ??n trang ??ng nh?p sau khi t?o thành công
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}