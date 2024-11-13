using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SubjectPage
{
    public class UpdateSubjectModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api";

        public UpdateSubjectModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UpdateSubjectModel SubjectData { get; set; } // Change this to your actual view model  

        [BindProperty]
        public string SubjectId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Subject/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            SubjectData = JsonConvert.DeserializeObject<UpdateSubjectModel>(jsonString);

            if (SubjectData == null)
            {
                return NotFound();
            }

            return Page(); // Return the page with the current model data  
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // If the model state is invalid, return the page with the current data  
            }

            // Create an HTTP client to send the update request  
            var client = _httpClientFactory.CreateClient();
            var jsonContent = JsonConvert.SerializeObject(SubjectData);
            var contentString = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Send a PUT request to update the subject using the API  
            var response = await client.PutAsync($"{_apiBaseUrl}/Subject/{SubjectData.Id}", contentString);

            if (response.IsSuccessStatusCode)
            {
                // If successful, redirect to the subject index page  
                TempData["SuccessMessage"] = "Subject updated successfully!";
                return RedirectToPage("/SubjectPage/Index");
            }

            // If the update failed, add model error and re-display the page  
            ModelState.AddModelError(string.Empty, "Failed to update subject. Please try again.");
            return Page();
        }
    }
}