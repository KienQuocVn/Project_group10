using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;

namespace OnDemandTutor.API.Pages.SchedulePage
{
    public class UpdateScheduleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api";

        public UpdateScheduleModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UpdateScheduleModelViews ScheduleData { get; set; }

        [BindProperty]
        public string ScheduleId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Schedule?pageNumber=1&pageSize=5&id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BasePaginatedList<ResponseScheduleModelViews>>(jsonString);

            if (result?.Items == null || !result.Items.Any())
            {
                return NotFound();
            }

            var scheduleData = result.Items.First();
            ScheduleId = scheduleData.Id;

            ScheduleData = new UpdateScheduleModelViews
            {
                StudentId = scheduleData.StudentId,
                SlotId = scheduleData.SlotId ?? string.Empty,
                Status = scheduleData.Status
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
            var response = await client.PutAsJsonAsync($"{_apiBaseUrl}/Schedule/{ScheduleId}", ScheduleData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/SchedulePage/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to update schedule: {errorContent}");
            return Page();
        }
    }
}
