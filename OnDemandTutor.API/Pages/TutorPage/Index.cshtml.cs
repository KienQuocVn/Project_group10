using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.TutorPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public BasePaginatedList<TutorSubject> PaginatedTutors { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public Guid? TutorId { get; set; }
        public string SubjectId { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 5, Guid? tutorId = null, string? subjectId = null)
        {
            // T?o URL v?i c�c tham s? truy v?n
            string apiUrl = $"https://localhost:7299/api/Tutor?pageNumber={pageNumber}&pageSize={pageSize}";

            if (tutorId.HasValue)
                apiUrl += $"&tutorId={tutorId}";
            if (!string.IsNullOrEmpty(subjectId))
                apiUrl += $"&subjectId={subjectId}";

            // G?i API v� l?y d? li?u
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                PaginatedTutors = JsonConvert.DeserializeObject<BasePaginatedList<TutorSubject>>(jsonString);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kh�ng th? t?i danh s�ch gia s? t? API.");
            }

            // C?p nh?t c�c tham s? t�m ki?m v� ph�n trang
            PageNumber = pageNumber;
            PageSize = pageSize;
            TutorId = tutorId;
            SubjectId = subjectId;
        }
    }
}
