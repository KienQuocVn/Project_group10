using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IComplaintService
    {
        Task<BasePaginatedList<Complaint>> GetAllComplaintsAsync(int pageNumber, int pageSize);
        Task<Complaint> GetComplaintByIdAsync(Guid id);
        Task<Complaint> CreateComplaintAsync(CreateComplaintModel model);
        Task<bool> UpdateComplaintAsync(Guid id, UpdateComplaintModel model);
        Task<bool> DeleteComplaintAsync(Guid id);
    }
}
