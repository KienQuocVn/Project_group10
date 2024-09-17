using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Repositories.UOW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;



        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BasePaginatedList<Schedule>> GetAllSchedulesAsync(int pageNumber, int pageSize)
        {
            // Lấy tất cả các bản ghi trong bảng Schedule
            IQueryable<Schedule> schedulesQuery = _unitOfWork.GetRepository<Schedule>().Entities.Include(p => p.Student).Where(p => !p.DeletedTime.HasValue).OrderByDescending(p => p.CreatedTime);

            // Đếm tổng số bản ghi
            int totalCount = await schedulesQuery.CountAsync();

            // Lấy dữ liệu phân trang
            var schedules = await schedulesQuery
                .OrderBy(s => s.Id)  // Sắp xếp theo trường ScheduleId (hoặc trường phù hợp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về đối tượng phân trang
            return new BasePaginatedList<Schedule>(schedules, totalCount, pageNumber, pageSize);
        }


        public async Task<Schedule> GetScheduleByIdAsync(String id)
        {
            // Sử dụng repository để lấy lịch theo ID
            return await _unitOfWork.ScheduleRepository.GetByIdAsync(id);
        }

        public async Task<Schedule> CreateScheduleAsync(CreateScheduleModelViews model)
        {
            // Tạo một thực thể Schedule mới từ model
            var schedule = new Schedule
            {
                Id = Guid.NewGuid().ToString("N"),
                // Student = await _unitOfWork.AccountRepository.GetByIdAsync(model.StudentId), // Lấy Student từ AccountRepository
                // Slot = await _unitOfWork.SlotRepository.GetByIdAsync(model.SlotId), // Lấy Slot từ SlotRepository
                // CreatedBy = Accounts.id;
                Status = "Created",
                CreatedTime = DateTimeOffset.Now,
                LastUpdatedTime = DateTimeOffset.Now
            };

            // Thêm thực thể Schedule vào cơ sở dữ liệu
            await _unitOfWork.ScheduleRepository.InsertAsync(schedule);
            await _unitOfWork.SaveAsync();

            return schedule;
        }

        public async Task<Schedule> UpdateScheduleAsync(String id,UpdateScheduleModelViews model)
        {
            // Lấy schedule từ database dựa trên Id
            var existingSchedule = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);

            if (existingSchedule != null)
            {
                // Cập nhật thông tin từ model vào entity
                existingSchedule.Status = model.Status;
                // existingSchedule.Student = await _unitOfWork.StudentRepository.GetByIdAsync(model.StudentId);
                // existingSchedule.Slot = await _unitOfWork.SlotRepository.GetByIdAsync(model.SlotId);
                // existingSchedule.LastUpdatedBy = Accounts.id;
                existingSchedule.LastUpdatedTime = DateTimeOffset.Now;
                

                // Cập nhật vào database
                _unitOfWork.ScheduleRepository.Update(existingSchedule);
                await _unitOfWork.SaveAsync();
            }
            return existingSchedule;
        }

        public async Task<bool> DeleteScheduleAsync(String id)
        {
            var existingProduct = await _unitOfWork.ScheduleRepository.GetByIdAsync(id);
            if (existingProduct != null) {
                // Xóa lịch theo ID
                await _unitOfWork.ScheduleRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
            
        }


    }
}
