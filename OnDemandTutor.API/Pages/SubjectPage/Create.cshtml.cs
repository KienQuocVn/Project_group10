using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.ModelViews.SubjectModelViews;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SubjectPage
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public CreateSubjectModelViews CreateSubject { get; set; } = new CreateSubjectModelViews();

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Send data to the API  
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7299/api/subject", CreateSubject);

            if (response.IsSuccessStatusCode)
            {
                // Redirect to the subject list page if successful  
                return RedirectToPage("/SubjectPage/Index");
            }
            else
            {
                // Handle error and display detailed error message  
                var strResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Error Response: " + strResponse);
                ModelState.AddModelError(string.Empty, strResponse);
                return Page();
            }
        }
    }
}