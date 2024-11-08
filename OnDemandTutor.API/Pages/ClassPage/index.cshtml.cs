using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ClassModelViews;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.ClassPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public BasePaginatedList<Class> PaginatedClasses { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string ClassId { get; set; }
        public Guid? AccountId { get; set; }
        public string SubjectId { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 5, string? classId = null, Guid? accountId = null, string? subjectId = null, DateTime? startDay = null, DateTime? endDay = null)
        {
            // Tạo URL với các tham số truy vấn
            string apiUrl = $"https://localhost:7299/api/Class?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(classId))
                apiUrl += $"&classId={classId}";
            if (accountId.HasValue)
                apiUrl += $"&accountId={accountId}";
            if (!string.IsNullOrEmpty(subjectId))
                apiUrl += $"&subjectId={subjectId}";
            if (startDay.HasValue)
                apiUrl += $"&startDay={startDay:yyyy-MM-dd}";
            if (endDay.HasValue)
                apiUrl += $"&endDay={endDay:yyyy-MM-dd}";

            // Gọi API và lấy dữ liệu
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                // Đọc dữ liệu từ API và gán cho PaginatedClasses
                var jsonString = await response.Content.ReadAsStringAsync();
                PaginatedClasses = JsonConvert.DeserializeObject<BasePaginatedList<Class>>(jsonString);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách lớp học từ API.");
            }

            // Cập nhật các tham số tìm kiếm và phân trang
            PageNumber = pageNumber;
            PageSize = pageSize;
            ClassId = classId;
            AccountId = accountId;
            SubjectId = subjectId;
            StartDay = startDay;
            EndDay = endDay;
        }
    }
}
