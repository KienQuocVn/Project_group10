using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.FeedbackModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IFeedbackService
    {
        Task<BasePaginatedList<Feedback>> GetDeleteAtFeedbackAsync(int pageNumber, int pageSize, Guid? studentId, Guid? tutorId, string? feedbackId);
        Task<BasePaginatedList<Feedback>> GetFeedbackByFilterAsync(int pageNumber, int pageSize, Guid? studentId, Guid? tutorId, string? feedbackId);
        Task<Feedback> CreateFeedbackAsync(CreateFeedbackModelViews model);
        Task<Feedback> UpdateFeedbackAsync(string id, Guid studentId, UpdateFeedbackModelViews model);
        Task<bool> DeleteFeedbackAsync(string id, Guid studentId);
    }
}
