using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;


namespace OnDemandTutor.API.Pages.Salary
{
    public class ProcessMonthEndSalaryModel : PageModel
    {
        private readonly IUserService _userService;

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public bool IsProcessed { get; set; } = false;

        public ProcessMonthEndSalaryModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            // Initial page load
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                // Process month-end salary for all tutors
                var processedCount = await _userService.ProcessMonthEndSalaryAsync();

                // Set success message
                SuccessMessage = $"Salary processed successfully for {processedCount} tutors.";
                IsProcessed = true;

                return Page();
            }
            catch (Exception ex)
            {
                // Set error message
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
