using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SchedulePage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public BasePaginatedList<Schedule> PaginatedSchedules { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? id { get; set; }
        public Guid? StudentId { get; set; }
        public string SlotId { get; set; }
        public string Status { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 5,string? id = null, Guid? studentId = null, string slotId = null, string status = null)
        {
            // Tạo URL với các tham số truy vấn
            string apiUrl = $"https://localhost:7299/api/Schedule?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(id))
                apiUrl += $"&id={id}";
            if (studentId.HasValue)
                apiUrl += $"&studentId={studentId}";
            if (!string.IsNullOrEmpty(slotId))
                apiUrl += $"&slotId={slotId}";
            if (!string.IsNullOrEmpty(status))
                apiUrl += $"&status={status}";

            // Gọi API và lấy dữ liệu
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                PaginatedSchedules = JsonConvert.DeserializeObject<BasePaginatedList<Schedule>>(jsonString);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách lịch học từ API.");
            }

            // Cập nhật các tham số tìm kiếm và phân trang
            PageNumber = pageNumber;
            PageSize = pageSize;
            StudentId = studentId;
            SlotId = slotId;
            Status = status;
        }
    }
}
