using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SLotModelViews;

namespace OnDemandTutor.API.Pages.Slots
{
    public class UpdateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7299/api";

        public UpdateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public SlotModelView SlotData { get; set; }

        [BindProperty]
        public string SlotId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/Slot/filter?pageNumber=1&pageSize=12&id={id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {jsonString}");

            var result = JsonConvert.DeserializeObject<BasePaginatedList<ResponseSlotModelView>>(jsonString);

            if (result?.Items == null || !result.Items.Any())
            {
                return NotFound();
            }

            var slotData = result.Items.First();
            SlotId = slotData.Id;

            SlotData = new SlotModelView
            {
                ClassId = slotData.ClassId,
                DayOfSlot = slotData.DayOfSlot ?? string.Empty,
                StartTime = slotData.StartTime,
                EndTime = slotData.EndTime,
                Price = slotData.Price
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
            var response = await client.PutAsJsonAsync($"{_apiBaseUrl}/Slot/update/{SlotId}", SlotData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Slots/Index");
            }

            // Log thông tin lỗi ra Console
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to update slot. Status Code: {response.StatusCode}, Error Content: {errorContent}");

            // Hiển thị thông báo lỗi cho người dùng trên giao diện
            ModelState.AddModelError("", $"Failed to update slot: {errorContent}");

            return Page();
        }
    }
}
