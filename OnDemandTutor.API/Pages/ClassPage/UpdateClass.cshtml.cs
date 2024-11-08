using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ClassModelViews;

namespace OnDemandTutor.API.Pages.ClassPage
{
    public class UpdateClassModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api";

        public UpdateClassModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UpdateClassModelView ClassData { get; set; }

        [BindProperty]
        public string ClassId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Class?pageNumber=1&pageSize=5&id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            // Debug: In ra JSON response để kiểm tra
            Console.WriteLine($"API Response: {jsonString}");

            var result = JsonConvert.DeserializeObject<BasePaginatedList<ResponseClassModelView>>(jsonString);

            if (result?.Items == null || !result.Items.Any())
            {
                return NotFound();
            }

            var classData = result.Items.First();
            ClassId = classData.Id;

            // Kiểm tra và xử lý dữ liệu an toàn hơn
            ClassData = new UpdateClassModelView
            {
                AccountId = classData.AccountId,
                SubjectId = classData.SubjectId ?? string.Empty,
                AmountOfSlot = classData.AmountOfSlot,
                StartDay = classData.StartDay,
                EndDay = classData.EndDay
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
            var response = await client.PutAsJsonAsync($"{_apiBaseUrl}/Class/{ClassId}", ClassData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/ClassPage/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to update class: {errorContent}");
            return Page();
        }
    }
}
