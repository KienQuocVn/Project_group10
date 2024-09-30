using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SLotModelViews;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Repositories.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class SlotService : ISlotSevice
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SlotService(IUnitOfWork unitOfWork, IMapper mapper) { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Slot> CreateSlotAsync(SlotModelView model)
        {
            bool isExistClass = await _unitOfWork.GetRepository<Class>().Entities
                .AnyAsync(s => s.Id == model.ClassId && !s.DeletedTime.HasValue);
            // Kiểm tra class có tồn tại hay không
            if (!isExistClass) {
                throw new Exception("Không tìm thấy class! Hãy thử lại");
            }

            //if (string.IsNullOrEmpty(model.ClassId) || !Guid.TryParse(model.ClassId, out Guid classGuid))
            //{
            //    throw new ArgumentException("ClassId không hợp lệ.");
            //}

            // Kiểm tra định dạng StartTime và EndTime
            if (!TimeSpan.TryParse(model.StartTime, out TimeSpan startTime))
            {
                throw new FormatException("Định dạng StartTime phải là 00:00:00. ");
            }

            if (!TimeSpan.TryParse(model.EndTime, out TimeSpan endTime))
            {
                throw new FormatException("Định dạng EndTime phải là 00:00:00. ");
            }

            // Kiểm tra xem StartTime có nhỏ hơn EndTime không
            if (startTime >= endTime)
            {
                throw new ArgumentException("StartTime phải nhỏ hơn EndTime.");
            }

            var slot = _mapper.Map<Slot>(model);

            if (slot == null)
            {
                throw new Exception("Lỗi khi map dữ liệu từ SlotModelView sang Slot.");
            }
            // Thiết lập các thuộc tính còn lại
            slot.Id = Guid.NewGuid().ToString("N");
            slot.CreatedBy = "admin";  // Ví dụ: lấy từ thông tin xác thực
            slot.CreatedTime = DateTimeOffset.Now;
            slot.LastUpdatedTime = DateTimeOffset.Now;

            // Thêm thực thể slot vào cơ sở dữ liệu
            await _unitOfWork.SlotRepository.InsertAsync(slot);
            // Kiểm tra slot trước khi lưu
            Console.WriteLine($"SlotId: {slot.Id}, StartTime: {slot.StartTime}, EndTime: {slot.EndTime}");

            await _unitOfWork.SaveAsync();

            return slot;
        }

        public async Task<bool> DeleteSlotAsync(string id)
        {
            var slot = await _unitOfWork.SlotRepository.Entities
                .FirstOrDefaultAsync(s => s.Id == id && !s.DeletedTime.HasValue);

            if (slot != null)
            {
                slot.DeletedTime = DateTimeOffset.Now;
                _unitOfWork.SlotRepository.Update(slot);
                await _unitOfWork.SaveAsync();

                return true; // Trả về true nếu xóa thành công
            }
            return false;
        }

        public async Task<BasePaginatedList<Slot>> GetAllSlotByFilterAsync(int pageNumber, int pageSize, string? classId, string? dayOfSlot, TimeSpan? StartTime, TimeSpan? endTime, double? price)
        {
            IQueryable<Slot> SlotQuerys = _unitOfWork.GetRepository<Slot>().Entities
                           .Where(p => !p.DeletedTime.HasValue) // Lọc Slot chưa bị xóa mềm
                           .OrderByDescending(p => p.CreatedTime); 

            // Điều kiện tìm kiếm class
            if (!string.IsNullOrEmpty(classId))
            {
                SlotQuerys = SlotQuerys.Where(p => p.ClassId == classId);
            }

            //return Slot;
            int totalCount = await SlotQuerys.CountAsync();
            var slot = await SlotQuerys
                .Skip((pageNumber - 1) * pageSize) // Phân trang
                .Take(pageSize)
                .ToListAsync();
            if (slot == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Slot với những thông tin trên.");
            }
            return new BasePaginatedList<Slot>(slot, totalCount, pageNumber, pageSize);
        }

        public async Task<Slot> UpdateSlotAsync(string id, SlotModelView model)
        {

            var slot = await _unitOfWork.SlotRepository.Entities
                .FirstOrDefaultAsync(s => s.Id == id && !s.DeletedTime.HasValue);

            // kiểm tra slot có tồn tại hay không
            if (slot == null) {
                throw new Exception("Không tìm thấy slot! Hãy thử lại");
            }

            _mapper.Map(model, slot);
            slot.LastUpdatedBy = "admin"; // ví dụ: lấy thông tin từ xác thực
            slot.LastUpdatedTime = DateTimeOffset.Now; // Cập nhật thời gian sửa đổi

            await _unitOfWork.SaveAsync();

            return slot;
        }
    }
}
