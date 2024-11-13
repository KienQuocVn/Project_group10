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
            // L?y chi ti?t c?a khi?u n?i theo ID ?? hi?n th? x�c nh?n x�a
            Complaint = await _complaintService.GetComplaintByIdAsync(id);
            if (Complaint == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            // G?i service ?? x�a khi?u n?i
            var response = await _complaintService.DeleteComplaintAsync(id);

            if (response == null)
            {
                ModelState.AddModelError(string.Empty, "Kh�ng th? x�a khi?u n?i.");
                return Page();
            }

            // Chuy?n h??ng v? trang Index sau khi x�a th�nh c�ng
            return RedirectToPage("/ComplaintPage/Index");
        }
    }
}
