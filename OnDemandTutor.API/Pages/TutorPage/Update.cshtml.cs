using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;

namespace OnDemandTutor.API.Pages.TutorPage
{
    public class UpdateTutorModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api";

        public UpdateTutorModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UpdateTutorSubjectModelViews TutorData { get; set; }

        [BindProperty]
        public string TutorId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Tutor?pageNumber=1&pageSize=5&id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BasePaginatedList<ResponseTutorModelViews>>(jsonString);

            if (result?.Items == null || !result.Items.Any())
            {
                return NotFound();
            }

            var tutorData = result.Items.First();
            TutorId = tutorData.TutorId.ToString();

            TutorData = new UpdateTutorSubjectModelViews
            {
                UserId = tutorData.UserId,
                SubjectId = tutorData.SubjectId ?? string.Empty,
                Bio = tutorData.Bio,
                Rating = tutorData.Rating,
                HourlyRate = tutorData.HourlyRate,
                Experience = tutorData.Experience
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.PutAsJsonAsync($"{_apiBaseUrl}/Tutor/{TutorId}", TutorData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/TutorPage/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to update tutor: {errorContent}");
            return Page();
        }
    }
}
