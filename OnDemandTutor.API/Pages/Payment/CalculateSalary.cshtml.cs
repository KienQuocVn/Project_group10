using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.UserModelViews;

namespace OnDemandTutor.API.Pages.Salary
{
    public class CalculateMonthSalaryModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public Guid TutorId { get; set; }

        [BindProperty]
        public int Month { get; set; }

        [BindProperty]
        public int Year { get; set; }

        [BindProperty]
        public string? ErrorMessage { get; set; }

        public SalaryCalculationViewModel? SalaryResult { get; set; }

        public CalculateMonthSalaryModel(IUserService userService)
        {
            _userService = userService;
            // Default to current month and year
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
        }

        public void OnGet()
        {
            // Initial page load
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                // Validate input
                if (TutorId == Guid.Empty)
                {
                    ErrorMessage = "Please provide a valid Tutor ID.";
                    return Page();
                }

                // Calculate salary
                double salary = await _userService.CalculateMonthSalaryAsync(TutorId, Month, Year);

                // Create view model for result
                SalaryResult = new SalaryCalculationViewModel
                {
                    TutorId = TutorId,
                    Salary = salary
                };

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error calculating salary: {ex.Message}";
                return Page();
            }
        }
    }

    // View model for salary calculation
    public class SalaryCalculationViewModel
    {
        public Guid TutorId { get; set; }
        public double Salary { get; set; }
    }
}
