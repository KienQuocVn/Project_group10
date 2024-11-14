using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.ModelViews.SLotModelViews;
using System.Net.Http.Json;

namespace OnDemandTutor.API.Pages.Slots
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public SlotModelView CreateSlot { get; set; } = new SlotModelView();

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {


            // Gửi dữ liệu đến API
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/slot", CreateSlot);



            if (response.IsSuccessStatusCode)
            {
                // Điều hướng đến trang danh sách nếu thành công
                return RedirectToPage("/Slots/Index");
            }
            else
            {
                // Xử lý lỗi và hiển thị thông báo lỗi chi tiết
                var strResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Error Response: " + strResponse);  // Ghi log lỗi ra console
                ModelState.AddModelError(string.Empty, strResponse);       // Hiển thị lỗi trên giao diện
                return Page();
            }
        }
    }
}
