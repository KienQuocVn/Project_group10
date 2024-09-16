using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;
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
            var query = _unitOfWork.ScheduleRepository.Entities;

            // Đếm tổng số bản ghi
            int totalCount = query.Count();

            // Lấy dữ liệu phân trang
            var schedules = await query
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

        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            // Thêm lịch mới vào cơ sở dữ liệu
            await _unitOfWork.ScheduleRepository.InsertAsync(schedule);
            await _unitOfWork.SaveAsync();
            return schedule;
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            // Cập nhật thông tin lịch hiện tại
            _unitOfWork.ScheduleRepository.Update(schedule);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteScheduleAsync(String id)
        {
            // Xóa lịch theo ID
            await _unitOfWork.ScheduleRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }


    }
}
