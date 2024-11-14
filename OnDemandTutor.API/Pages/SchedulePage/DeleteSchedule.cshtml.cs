using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SchedulePage
{
    public class DeleteScheduleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api"; // URL API của bạn

        public DeleteScheduleModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string ScheduleId { get; set; }

        // Xử lý GET để nhận ID Schedule
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ScheduleId = id; // Gán giá trị ID từ URL vào thuộc tính ScheduleId
            return Page();
        }

        // Xử lý POST để xóa Schedule
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(ScheduleId))
            {
                return BadRequest("Schedule ID is required");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiBaseUrl}/Schedule/{ScheduleId}");

            if (response.IsSuccessStatusCode)
            {
                // Sau khi xóa thành công, chuyển hướng về danh sách Schedule
                return RedirectToPage("/SchedulePage/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to delete schedule: {errorContent}");
            return Page();
        }
    }
}
