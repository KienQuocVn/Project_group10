using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using System.Net.Http.Json;

namespace OnDemandTutor.API.Pages.TutorPage
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public CreateTutorSubjectModelViews CreateTutor { get; set; } = new CreateTutorSubjectModelViews();

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // G?i d? li?u ??n API
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/tutor", CreateTutor);

            if (response.IsSuccessStatusCode)
            {
                // ?i?u h??ng ??n trang danh sách n?u thành công
                return RedirectToPage("/TutorPage/Index");
            }
            else
            {
                // X? lý l?i và hi?n th? thông báo l?i chi ti?t
                var strResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Error Response: " + strResponse);
                ModelState.AddModelError(string.Empty, strResponse);
                return Page();
            }
        }
    }
}
