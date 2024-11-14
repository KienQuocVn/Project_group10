using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Pages.ComplaintPage
{
    public class IndexComplaintModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        public IndexComplaintModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // Thay đổi kiểu dữ liệu từ List<ResponseComplaintModel> sang List<ResponseComplaintModel>
        public List<ResponseComplaintModel> Complaints { get; set; }

        public async Task OnGetAsync(string studentId = null, string tutorId = null)
        {
            // Chuyển đổi từ string sang Guid nếu có giá trị
            Guid? studentGuid = null;
            Guid? tutorGuid = null;

            if (!string.IsNullOrEmpty(studentId) && Guid.TryParse(studentId, out Guid parsedStudentId))
            {
                studentGuid = parsedStudentId;
            }

            if (!string.IsNullOrEmpty(tutorId) && Guid.TryParse(tutorId, out Guid parsedTutorId))
            {
                tutorGuid = parsedTutorId;
            }

            // Lấy danh sách khiếu nại từ service (BasePaginatedList<Complaint>)
            var paginatedComplaints = await _complaintService.GetAllComplaintsAsync(1, 10, studentGuid, tutorGuid, null);

            // Chuyển đổi dữ liệu từ BasePaginatedList<Complaint> sang List<ResponseComplaintModel>
            Complaints = paginatedComplaints.Items
                .Select(c => new ResponseComplaintModel
                {
                    // Kiểm tra và chuyển đổi Id nếu cần thiết
                    Id = Guid.TryParse(c.Id.ToString(), out Guid complaintId) ? complaintId : Guid.Empty,
                    StudentId = c.StudentId,
                    TutorId = c.TutorId,
                    Content = c.Content,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt
                })
                .ToList();
        }
    }
}
