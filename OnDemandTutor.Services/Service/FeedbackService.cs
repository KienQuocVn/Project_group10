using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.FeedbackModelViews;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackModelViews model)
        {
            // Tạo một thực thể Feedback mới từ model
            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString("N"),
                StudentId = model.StudentId,
                TutorId = model.TutorId,
                FeedbackText = model.FeedbackText,
                CreatedTime = DateTimeOffset.Now,
                LastUpdatedTime = DateTimeOffset.Now
            };

            // Thêm thực thể Feedback vào cơ sở dữ liệu
            await _unitOfWork.FeedbackRepository.InsertAsync(feedback);
            await _unitOfWork.SaveAsync();

            return feedback;
        }

        public async Task<bool> DeleteFeedbackAsync(string id, Guid studentId)
        {
            var existingFeedback = await _unitOfWork.FeedbackRepository.Entities
                .FirstOrDefaultAsync(feedback => feedback.Id == id && feedback.StudentId == studentId);
            if (existingFeedback != null)
            {
                existingFeedback.DeletedTime = DateTimeOffset.Now;
                _unitOfWork.FeedbackRepository.Update(existingFeedback);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<BasePaginatedList<Feedback>> GetAllFeedbackAsync(int pageNumber, int pageSize)
        {
            IQueryable<Feedback> FeedbacksQuery = _unitOfWork.GetRepository<Feedback>().Entities.Where(p => !p.DeletedTime.HasValue).OrderByDescending(p => p.CreatedTime);
            int totalCount = await FeedbacksQuery.CountAsync();
            var feedback = await FeedbacksQuery
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new BasePaginatedList<Feedback>(feedback, totalCount, pageNumber, pageSize);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(string id)
        {
            return await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
        }
        public async Task<bool> UpdateFeedbackAsync(string id, Guid studentId, UpdateFeedbackModelViews Feedback)
        {
            // Tìm feedback dựa trên feedbackId và studentId
            var feedback = await _unitOfWork.FeedbackRepository.Entities
                .FirstOrDefaultAsync(feedback => feedback.Id == id && feedback.StudentId == studentId);

            // Nếu feedback không tồn tại hoặc không phải của student này, trả về false
            if (feedback == null)
            {
                return false;
            }

            feedback.FeedbackText = Feedback.FeedbackText;
            feedback.LastUpdatedTime = DateTimeOffset.Now;

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}