using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.ComplaintPage
{
    public class DeleteComplaintModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        public DeleteComplaintModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        [BindProperty]
        public ResponseComplaintModel Complaint { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // L?y chi ti?t c?a khi?u n?i theo ID ?? hi?n th? xác nh?n xóa
            Complaint = await _complaintService.GetComplaintByIdAsync(id);
            if (Complaint == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            // G?i service ?? xóa khi?u n?i
            var response = await _complaintService.DeleteComplaintAsync(id);

            if (response == null)
            {
                ModelState.AddModelError(string.Empty, "Không th? xóa khi?u n?i.");
                return Page();
            }

            // Chuy?n h??ng v? trang Index sau khi xóa thành công
            return RedirectToPage("/ComplaintPage/Index");
        }
    }
}
