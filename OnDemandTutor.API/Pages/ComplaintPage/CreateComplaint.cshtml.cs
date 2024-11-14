using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using OnDemandTutor.Services;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.Pages.ComplaintPage
{
    public class CreateComplaintPageModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        // Thuộc tính này được sử dụng để nhận dữ liệu từ form
        [BindProperty]
        public CreateComplaintInputModel ComplaintInput { get; set; } = new CreateComplaintInputModel();

        public CreateComplaintPageModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public void OnGet()
        {
            // Xử lý khi trang được tải lần đầu
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tạo mới đối tượng `CreateComplaintModel` từ `ComplaintInput`
            var newComplaint = new CreateComplaintModel
            {
                StudentId = ComplaintInput.StudentId,
                TutorId = ComplaintInput.TutorId,
                Content = ComplaintInput.Content,
                Status = ComplaintInput.Status
            };

            // Gọi service để xử lý tạo mới complaint
            await _complaintService.CreateComplaintAsync(newComplaint);

            // Chuyển hướng về trang Index sau khi hoàn tất
            return RedirectToPage("Index");
        }
    }

    // Lớp input model dùng cho binding từ form
    public class CreateComplaintInputModel
    {
        public Guid StudentId { get; set; }
        public Guid TutorId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
    }
}
