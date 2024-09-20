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
        Task<BasePaginatedList<Feedback>> GetAllFeedbackAsync(int pageNumber, int pageSize);
        Task<Feedback> GetFeedbackByIdAsync(string id);
        Task<Feedback> CreateFeedbackAsync(CreateFeedbackModelViews model);
        Task<bool> UpdateFeedbackAsync(string id, Guid studentId, UpdateFeedbackModelViews Feedback);
        Task<bool> DeleteFeedbackAsync(string id, Guid studentId);
    }
}
