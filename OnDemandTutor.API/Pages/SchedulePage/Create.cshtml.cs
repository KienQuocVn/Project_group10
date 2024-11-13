using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.ScheduleModelViews;
using OnDemandTutor.Services;

namespace OnDemandTutor.API.Pages.SchedulePage
{
    public class CreateModel : PageModel
    {
        private readonly IScheduleService _scheduleService;

        [BindProperty]
        public CreateScheduleModelViews CreateSchedule { get; set; }

        public CreateModel(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public void OnGet()
        {
            // Thực hiện các thao tác cần thiết khi load trang
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Tạo mới Schedule dựa trên thông tin từ form
                ResponseScheduleModelViews result = await _scheduleService.CreateScheduleAsync(CreateSchedule);

                // Điều hướng về trang danh sách hoặc trang chi tiết nếu cần
                return RedirectToPage("/SchedulePage/Index");
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có ngoại lệ xảy ra
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
