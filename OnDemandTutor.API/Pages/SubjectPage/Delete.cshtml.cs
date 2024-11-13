using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SubjectPage
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api";

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string SubjectId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            SubjectId = id; // Assuming you need to retrieve more details about the subject, you can consider adding API calls here to fetch further data.  
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(SubjectId))
            {
                return BadRequest("Subject ID is required");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_apiBaseUrl}/subject/{SubjectId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/SubjectPage/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to delete subject: {errorContent}");
            return Page();
        }
    }
}