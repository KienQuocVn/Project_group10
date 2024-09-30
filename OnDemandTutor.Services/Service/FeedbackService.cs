using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.FeedbackModelViews;
using OnDemandTutor.ModelViews.ScheduleModelViews;
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
        private IMapper _mapper;
        private readonly UserManager<Accounts> _userManager;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Accounts> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager; // Inject UserManager<Accounts>
        }

        public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackModelViews model)
        {
            // Tìm user dựa trên StudentId
            var student = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == model.StudentId && !s.DeletedTime.HasValue);

            // kiểm tra student có tồi tại hoặc đúng vai trò
            if (student == null || !await _userManager.IsInRoleAsync(student, "Student"))
            {
                throw new Exception("Không tìm thấy sinh viên! hãy thử lại");
            }

            // Tìm user dựa trên TutorId
            var tutor = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == model.TutorId && !s.DeletedTime.HasValue);

            // kiểm tra tutor có tồi tại hoặc đúng vai trò
            if (tutor == null || !await _userManager.IsInRoleAsync(tutor, "Tutor"))
            {
                throw new Exception("Không tìm thấy gia sư! hãy thử lại");
            }

            //Kiểm tra sự tồn tại của Slot
            bool isExistSlot = await _unitOfWork.GetRepository<Slot>().Entities
                .AnyAsync(s => s.Id == model.SlotId && !s.DeletedTime.HasValue);

            if (!isExistSlot)
            {
                throw new Exception("Không tìm thấy Slot! Hãy thử lại");
            }

            // Kiểm tra xem Student đã feedback cho Tutor này chưa
            var feedbackExists = await _unitOfWork.FeedbackRepository.Entities
                .AnyAsync(f => f.StudentId == model.StudentId && 
                f.TutorId == model.TutorId && 
               !f.DeletedTime.HasValue && 
                f.SlotId == model.SlotId);

            if (feedbackExists)
            {
                throw new Exception("Sinh viên đã phản hồi đến gia sư này.");
            }

            var feedback = _mapper.Map<Feedback>(model);

            // Thiết lập các thuộc tính còn lại
            feedback.Id = Guid.NewGuid().ToString("N");
            feedback.CreatedTime = DateTimeOffset.Now;
            feedback.LastUpdatedTime = DateTimeOffset.Now;

            // Thêm thực thể Feedback vào cơ sở dữ liệu
            await _unitOfWork.FeedbackRepository.InsertAsync(feedback);
            await _unitOfWork.SaveAsync();

            return feedback;
        }

        public async Task<BasePaginatedList<Feedback>> GetDeleteAtFeedbackAsync(int pageNumber, int pageSize, string? slotId, Guid? studentId, Guid? tutorId, string? feedbackId)
        {
            IQueryable<Feedback> FeedbacksQuery = _unitOfWork.GetRepository<Feedback>().Entities
                .Where(p => p.DeletedTime.HasValue)  // Lấy feedback đã bị xóa mềm
                .OrderByDescending(p => p.CreatedTime);
       
            // Điều kiện tìm kiếm theo StudentId, TutorId, FeedbackId
            if (studentId.HasValue && studentId != Guid.Empty)
                FeedbacksQuery = FeedbacksQuery.Where(p => p.StudentId == studentId);

            if (tutorId.HasValue && tutorId != Guid.Empty)
                FeedbacksQuery = FeedbacksQuery.Where(p => p.TutorId == tutorId);

            // kiểm tra xem slot có tồn tại hay không
            if (!string.IsNullOrEmpty(slotId))
            {
                FeedbacksQuery = FeedbacksQuery.Where(p => p.Slot.Id == slotId);
            }

            // kiểm tra xem feedback có tồn tại hay không
            if (!string.IsNullOrEmpty(feedbackId))
                FeedbacksQuery = FeedbacksQuery.Where(p => p.Id == feedbackId);

            int totalCount = await FeedbacksQuery.CountAsync();
            var feedback = await FeedbacksQuery
                .Skip((pageNumber - 1) * pageSize) // Phân trang
                .Take(pageSize)
                .ToListAsync();
            if (feedback == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Feedback với những thông tin trên.");
            }
            return new BasePaginatedList<Feedback>(feedback, totalCount, pageNumber, pageSize);
        }

        public async Task<BasePaginatedList<Feedback>> GetFeedbackByFilterAsync(int pageNumber, int pageSize ,string? slotId, Guid? studentId, Guid? tutorId, string? feedbackId)
        {
            IQueryable<Feedback> feedbackQuery = _unitOfWork.GetRepository<Feedback>().Entities
                .Where(p => !p.DeletedTime.HasValue);  // Lọc feedback chưa bị xóa mềm

            // Điều kiện tìm kiếm theo StudentId
            if (studentId.HasValue && studentId != Guid.Empty)
            {
                feedbackQuery = feedbackQuery.Where(p => p.StudentId == studentId.Value);
            }

            if (tutorId.HasValue && tutorId != Guid.Empty)
            {
                // Nếu không có studentId, tìm theo TutorId
                feedbackQuery = feedbackQuery.Where(p => p.TutorId == tutorId.Value);
            }
            // Điều kiện tìm kiếm theo FeedbackId nếu có
            if (!string.IsNullOrEmpty(slotId))
            {
                feedbackQuery = feedbackQuery.Where(p => p.Slot.Id == slotId);
            }
            // Điều kiện tìm kiếm theo FeedbackId nếu có
            if (!string.IsNullOrEmpty(feedbackId))
            {
                feedbackQuery = feedbackQuery.Where(p => p.Id == feedbackId);
            }


            //return feedback;
            int totalCount = await feedbackQuery.CountAsync();
            var feedback = await feedbackQuery
                .Skip((pageNumber - 1) * pageSize) // Phân trang
                .Take(pageSize)
                .ToListAsync();
            if (feedback == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Feedback với những thông tin trên.");
            }
            return new BasePaginatedList<Feedback>(feedback, totalCount, pageNumber, pageSize);
        }


        public async Task<Feedback> UpdateFeedbackAsync(string id, Guid studentId, UpdateFeedbackModelViews model)
        {
            // Tìm feedback dựa trên feedbackId và studentId
            var feedback = await _unitOfWork.FeedbackRepository.Entities
                .FirstOrDefaultAsync(f => f.Id == id && f.StudentId == studentId && !f.DeletedTime.HasValue);

            var student = await _unitOfWork.FeedbackRepository.Entities
              .FirstOrDefaultAsync(f => f.StudentId == studentId && !f.DeletedTime.HasValue);
            if (student == null)
            {
                throw new Exception("Không tìm thấy student! Hãy thử lại.");
            }

            // Nếu feedback không tồn tại hoặc không phải của sinh viên này, trả về thông báo lỗi
            if (feedback == null)
            {
                throw new Exception("Không tìm thấy feedback cho sinh viên này! Hãy thử lại.");
            }

            // Sử dụng AutoMapper để ánh xạ dữ liệu
            _mapper.Map(model, feedback);
            feedback.LastUpdatedTime = DateTimeOffset.Now; // Cập nhật thời gian sửa đổi

            await _unitOfWork.SaveAsync();

            return feedback;
        }




        public async Task<bool> DeleteFeedbackAsync(string id, Guid studentId)
        {
            // tìm kiếm id
            var existingFeedback = await _unitOfWork.FeedbackRepository.Entities
            .FirstOrDefaultAsync(f => f.Id == id && f.StudentId == studentId && !f.DeletedTime.HasValue);

            if (existingFeedback != null)
            {
                existingFeedback.DeletedTime = DateTimeOffset.Now;
                _unitOfWork.FeedbackRepository.Update(existingFeedback);
                await _unitOfWork.SaveAsync();

                return true; // Trả về true nếu xóa thành công
            }
            return false;
        }

    }
}