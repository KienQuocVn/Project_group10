using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using OnDemandTutor.Contract.Services.Interface;

namespace OnDemandTutor.API.Pages.ComplaintPage
{
    public class UpdateComplaintPageModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        // Constructor nhận vào IComplaintService
        public UpdateComplaintPageModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // Property chứa dữ liệu khiếu nại cần cập nhật
        [BindProperty]
        public UpdateComplaintModel Complaint { get; set; }

        // GET: Lấy thông tin khiếu nại cần sửa
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(id);

            if (complaint == null)
            {
                return NotFound(); // Nếu không tìm thấy khiếu nại
            }

            // Gán dữ liệu khiếu nại vào model
            Complaint = new UpdateComplaintModel
            {
                StudentId = complaint.StudentId,
                TutorId = complaint.TutorId,
                Content = complaint.Content,
                Status = complaint.Status
            };

            return Page();
        }

        // POST: Cập nhật khiếu nại
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();  // Nếu model không hợp lệ, giữ lại trang
            }

            // Gọi service để cập nhật khiếu nại
            var updatedComplaint = await _complaintService.UpdateComplaintAsync(Complaint.StudentId, Complaint);

            if (updatedComplaint == null)
            {
                return NotFound(); // Nếu cập nhật không thành công
            }

            // Redirect sau khi cập nhật thành công
            return RedirectToPage("/ComplaintPage/IndexComplaint");
        }
    }
}
