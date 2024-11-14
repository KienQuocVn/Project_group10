using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SlotPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public BasePaginatedList<Slot> PaginatedSlots { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string ClassId { get; set; }
        public string DayOfSlot { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public double? Price { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 12, string? classId = null, string? dayOfSlot = null, TimeSpan? startTime = null, TimeSpan? endTime = null, double? price = null)
        {
            string apiUrl = $"https://localhost:7299/api/Slot/filter?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(classId))
                apiUrl += $"&classId={classId}";
            if (!string.IsNullOrEmpty(dayOfSlot))
                apiUrl += $"&dayOfSlot={dayOfSlot}";
            if (startTime.HasValue)
                apiUrl += $"&startTime={startTime:hh\\:mm}";
            if (endTime.HasValue)
                apiUrl += $"&endTime={endTime:hh\\:mm}";
            if (price.HasValue)
                apiUrl += $"&price={price}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                PaginatedSlots = JsonConvert.DeserializeObject<BasePaginatedList<Slot>>(jsonString);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách slot từ API.");
            }

            PageNumber = pageNumber;
            PageSize = pageSize;
            ClassId = classId;
            DayOfSlot = dayOfSlot;
            StartTime = startTime;
            EndTime = endTime;
            Price = price;
        }
    }
}
