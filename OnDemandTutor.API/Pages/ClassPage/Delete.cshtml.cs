using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json; // Sử dụng System.Text.Json để phân tích cú pháp JSON
using System.Threading.Tasks;
using System.Net.Http;

namespace OnDemandTutor.API.Pages.ClassPage
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api"; // URL API của bạn

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string ClassId { get; set; }

        // Xử lý GET để nhận ID Class
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ClassId = id; // Gán giá trị ID từ URL vào thuộc tính ClassId
            return Page();
        }

        // Xử lý POST để xóa Class
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(ClassId))
            {
                return BadRequest("Class ID is required");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiBaseUrl}/Class/{ClassId}");

            if (response.IsSuccessStatusCode)
            {
                // Sau khi xóa thành công, chuyển hướng về danh sách Class
                return RedirectToPage("/ClassPage/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to delete class: {errorContent}");
            return Page();
        }
    }
}
