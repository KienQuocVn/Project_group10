using AutoMapper;
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
        private readonly IMapper _mapper;



        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }


        // Lấy danh sách Tất cả Schedule (xóa và chưa bị xóa) dựa theo các tham số truyền vào (nếu tham số nào null thì k tìm theo tham số đó)
        public async Task<BasePaginatedList<Schedule>> GetAllSchedulesAsync(int pageNumber, int pageSize, Guid? studentId, string? slotId, string? status)
        {
            // Lấy tất cả các bản ghi trong bảng Schedule với điều kiện tìm kiếm
            IQueryable<Schedule> schedulesQuery = _unitOfWork.GetRepository<Schedule>().Entities
                .OrderByDescending(p => p.CreatedTime);

            // Điều kiện tìm kiếm theo studentId nếu có
            if (studentId.HasValue)
            {
                schedulesQuery = schedulesQuery.Where(p => p.Student.Id == studentId);
            }

            // Điều kiện tìm kiếm theo slotId nếu có
            if (!string.IsNullOrWhiteSpace(slotId))
            {
                schedulesQuery = schedulesQuery.Where(p => p.Slot.Id == slotId);
            }

            // Điều kiện tìm kiếm theo status nếu có
            if (!string.IsNullOrWhiteSpace(status))
            {
                schedulesQuery = schedulesQuery.Where(p => p.Status == status);
            }

            // Đếm tổng số bản ghi
            int totalCount = await schedulesQuery.CountAsync();

            // Áp dụng phân trang và sắp xếp theo Id
            List<Schedule> paginatedSchedules = await schedulesQuery
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<Schedule>(paginatedSchedules, totalCount, pageNumber, pageSize);
        }

        // Lấy danh sách Schedule chưa bị xóa dựa theo các tham số truyền vào (nếu tham số nào null thì k tìm theo tham số đó)
        public async Task<BasePaginatedList<Schedule>> GetSchedulesByFilterAsync(int pageNumber, int pageSize, Guid? studentId, string? slotId, string? status)
        {
            // Lấy tất cả các bản ghi trong bảng Schedule với điều kiện tìm kiếm
            IQueryable<Schedule> schedulesQuery = _unitOfWork.GetRepository<Schedule>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            // Điều kiện tìm kiếm theo studentId nếu có
            if (studentId.HasValue)
            {
                schedulesQuery = schedulesQuery.Where(p => p.Student.Id == studentId);
            }

            // Điều kiện tìm kiếm theo slotId nếu có
            if (!string.IsNullOrWhiteSpace(slotId))
            {
                schedulesQuery = schedulesQuery.Where(p => p.Slot.Id == slotId);
            }

            // Điều kiện tìm kiếm theo status nếu có
            if (!string.IsNullOrWhiteSpace(status))
            {
                schedulesQuery = schedulesQuery.Where(p => p.Status == status);
            }

            // Đếm tổng số bản ghi
            int totalCount = await schedulesQuery.CountAsync();

            // Áp dụng phân trang và sắp xếp theo Id
            List<Schedule> paginatedSchedules = await schedulesQuery
                .OrderBy(s => s.Id) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<Schedule>(paginatedSchedules, totalCount, pageNumber, pageSize);
        }

        // Tìm kiếm lịch theo id
        public async Task<Schedule> GetScheduleByIdAsync(string id)
        {
            Schedule existingSchedule = await _unitOfWork.GetRepository<Schedule>().Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue) ?? throw new Exception("The Schedule can not found!");
            return _mapper.Map<ResponseScheduleModelViews>(schedule);

        }


        public async Task<ResponseScheduleModelViews> CreateScheduleAsync(CreateScheduleModelViews model)
        {
            // Kiểm tra các thuộc tính không được để trống


            if (model.StudentId == Guid.Empty)
            {
                throw new Exception("Please enter StudentId.");
            }

            if (string.IsNullOrWhiteSpace(model.SlotId))
            {
                throw new Exception("Please enter SlotId.");
            }

            // Kiểm tra sự tồn tại của Student
            bool isExistStudent = await _unitOfWork.GetRepository<Accounts>().Entities
                .AnyAsync(s => s.Id == model.StudentId && !s.DeletedTime.HasValue);

            if (!isExistStudent)
            {
                throw new Exception("The Student can not found!");
            }

            // Kiểm tra sự tồn tại của Slot
            bool isExistSlot = await _unitOfWork.GetRepository<Slot>().Entities
                .AnyAsync(s => s.Id == model.SlotId && !s.DeletedTime.HasValue);

            if (!isExistSlot)
            {
                throw new Exception("The Slot can not found!");
            }

            // Sử dụng AutoMapper để ánh xạ từ model sang thực thể Schedule
            var schedule = _mapper.Map<Schedule>(model);

            // Thiết lập thêm các thuộc tính không có trong CreateScheduleModelViews
            schedule.Id = Guid.NewGuid().ToString("N");
            schedule.CreatedBy = "claim account";  // Ví dụ: lấy từ thông tin xác thực
            schedule.CreatedTime = DateTimeOffset.UtcNow;
            schedule.LastUpdatedTime = DateTimeOffset.UtcNow;

            // Thêm thực thể Schedule vào cơ sở dữ liệu
            await _unitOfWork.ScheduleRepository.InsertAsync(schedule);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ResponseScheduleModelViews>(schedule);
        }



        public async Task<ResponseScheduleModelViews> UpdateScheduleAsync(string id, UpdateScheduleModelViews model)
        {

            if (model.StudentId == Guid.Empty)
            {
                throw new Exception("Please enter StudentId.");
            }

            if (string.IsNullOrWhiteSpace(model.SlotId))
            {
                throw new Exception("Please enter SlotId.");
            }

            // Lấy schedule từ database dựa trên Id
            Schedule existingSchedule = await _unitOfWork.GetRepository<Schedule>().Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue) ?? throw new Exception("The Schedule can not found!");


            // Kiểm tra sự tồn tại của Student
            bool isExistStudent = await _unitOfWork.GetRepository<Accounts>().Entities
                .AnyAsync(s => s.Id == model.StudentId && !s.DeletedTime.HasValue);

            if (!isExistStudent)
            {
                throw new Exception("The Student can not found!");
            }

            // Kiểm tra sự tồn tại của Slot
            bool isExistSlot = await _unitOfWork.GetRepository<Slot>().Entities
                .AnyAsync(s => s.Id == model.SlotId && !s.DeletedTime.HasValue);

            if (!isExistSlot)
            {
                throw new Exception("The Slot can not found!");
            }

            _mapper.Map(model, existingSchedule);

            // Thiết lập các thuộc tính bổ sung
            existingSchedule.LastUpdatedBy = "claim account";  // Lấy từ thông tin xác thực
            existingSchedule.LastUpdatedTime = DateTimeOffset.UtcNow;

            // Cập nhật thực thể Schedule vào database
            _unitOfWork.ScheduleRepository.Update(existingSchedule);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ResponseScheduleModelViews>(existingSchedule);
        }

        public async Task<ResponseScheduleModelViews> DeleteScheduleAsync(String id)
        {
            Schedule existingSchedule = await _unitOfWork.GetRepository<Schedule>().Entities.FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue) ?? throw new Exception("The Schedule can not found!");

            if (existingSchedule != null)
            {
                existingSchedule.DeletedTime = DateTimeOffset.UtcNow;
                existingSchedule.DeletedBy = "claim account";

                // Xóa lịch theo ID
                _unitOfWork.ScheduleRepository.Update(existingSchedule);
                await _unitOfWork.SaveAsync();
            }
            return _mapper.Map<ResponseScheduleModelViews>(existingSchedule);

        }
    }
}
