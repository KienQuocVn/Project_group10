using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.Slots
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api"; // Cập nhật URL API của bạn

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string SlotId { get; set; }

        // Lấy Slot ID từ URL và hiển thị trên trang xóa
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            SlotId = id;
            return Page();
        }

        // Xử lý xóa Slot khi người dùng submit form
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(SlotId))
            {
                return BadRequest("Slot ID is required");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiBaseUrl}/Slot/delete/{SlotId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Slots/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to delete slot: {errorContent}");
            return Page();
        }
    }
}
