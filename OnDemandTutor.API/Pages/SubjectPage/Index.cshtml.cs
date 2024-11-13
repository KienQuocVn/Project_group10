using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews; // Adjust this to match the correct namespace for Subject model views  
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.SubjectPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public BasePaginatedList<Subject> PaginatedSubjects { get; set; } // Adjust to the correct type for Subject  
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 5)
        {
            // Construct API URL for pagination  
            string apiUrl = $"https://localhost:7299/api/Subject?pageNumber={pageNumber}&pageSize={pageSize}";

            // Call the API and get the data  
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                PaginatedSubjects = JsonConvert.DeserializeObject<BasePaginatedList<Subject>>(jsonString); // Adjust to the correct type  
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to retrieve subjects from the API.");
            }

            // Update pagination parameters  
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}