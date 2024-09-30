using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IComplaintService
    {
        // Phương thức để lấy tất cả khiếu nại với phân trang
        Task<BasePaginatedList<Complaint>> GetAllComplaintsAsync(int pageNumber, int pageSize, Guid? studentId, Guid? tutorId, string? status);

        // Phương thức để lấy một khiếu nại theo ID
        Task<ResponseComplaintModel> GetComplaintByIdAsync(Guid id);

        // Phương thức để tạo một khiếu nại mới
        Task<ResponseComplaintModel> CreateComplaintAsync(CreateComplaintModel model);

        // Phương thức để cập nhật một khiếu nại theo ID
        Task<ResponseComplaintModel> UpdateComplaintAsync(Guid id, UpdateComplaintModel model);

        // Phương thức để xóa một khiếu nại theo ID
        Task<ResponseComplaintModel> DeleteComplaintAsync(Guid id);
    }
}
