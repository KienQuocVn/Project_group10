using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.ModelViews.ClassModelViews;
using System.Net.Http.Json; // Thêm thư viện này nếu chưa có

namespace OnDemandTutor.API.Pages.ClassPage
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public CreateClassModelView CreateClass { get; set; } = new CreateClassModelView();

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {


            // Gửi dữ liệu đến API
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/class", CreateClass);



            if (response.IsSuccessStatusCode)
            {
                // Điều hướng đến trang danh sách nếu thành công
                return RedirectToPage("/ClassPage/Index");
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
