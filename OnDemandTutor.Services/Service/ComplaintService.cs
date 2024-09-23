using System;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Core.Utils;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using Microsoft.EntityFrameworkCore;
using Castle.Components.DictionaryAdapter.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnDemandTutor.Services.Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ComplaintService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // lấy hết khiếu nại về rồi phân ra
        public async Task<BasePaginatedList<Complaint>> GetAllComplaintsAsync(int pageNumber, int pageSize)
        {
            var complaintQuery = _unitOfWork.GetRepository<Complaint>()
                .Entities
                .Where(c => !c.DeletedTime.HasValue)
                .OrderByDescending(c => c.CreatedAt);

            int totalCount = await complaintQuery.CountAsync();
            var complaints = await complaintQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<Complaint>(complaints, totalCount, pageNumber, pageSize);
        }

        // Hàm này tìm kiếm một khiếu nại dựa vào ID, trả về khiếu nại nếu tồn tại và chưa bị xóa. Nếu không tìm thấy hoặc khiếu nại đã bị xóa (có DeletedTime), trả về null.
        public async Task<Complaint> GetComplaintByIdAsync(Guid id)
        {
            var complaint = await _unitOfWork.GetRepository<Complaint>().GetByIdAsync(id);

            if (complaint == null || complaint.DeletedTime.HasValue)
            {
                return null;
            }

            return complaint;
        }

        // Hàm này nhận dữ liệu từ CreateComplaintModel, tạo một đối tượng Complaint mới với các thông tin như StudentId, TutorId, và Content.
        //Sau đó, nó lưu vào database thông qua phương thức InsertAsync và gọi SaveAsync() để xác nhận thao tác.
        public async Task<Complaint> CreateComplaintAsync(CreateComplaintModel model)
        {
            var newComplaint = new Complaint
            {
                StudentId = model.StudentId,
                TutorId = model.TutorId,
                Content = model.Content,
                CreatedAt = DateTime.UtcNow,
                Status = "New"
            };

            await _unitOfWork.GetRepository<Complaint>().InsertAsync(newComplaint);
            await _unitOfWork.SaveAsync();

            return newComplaint;
        }

        // Hàm này tìm khiếu nại theo ID, nếu tìm thấy và chưa bị xóa, nó cập nhật nội dung khiếu nại với thông tin mới từ UpdateComplaintModel.
        //Sau đó, lưu thay đổi bằng cách gọi phương thức Update và SaveAsync(). Trả về true nếu cập nhật thành công, ngược lại trả về false.
        public async Task<bool> UpdateComplaintAsync(Guid id, UpdateComplaintModel model)
        {
            var existingComplaint = await _unitOfWork.GetRepository<Complaint>().GetByIdAsync(id);

            if (existingComplaint == null || existingComplaint.DeletedTime.HasValue)
            {
                return false; //
            }

            existingComplaint.Content = model.Content;
            existingComplaint.Status = model.Status;




            existingComplaint.LastUpdatedTime = CoreHelper.SystemTimeNow;

            _unitOfWork.GetRepository<Complaint>().Update(existingComplaint);
            await _unitOfWork.SaveAsync();

            return true;
        }

        //Hàm này thực hiện "soft delete", nghĩa là không xóa vật lý bản ghi khỏi database, mà chỉ cập nhật thuộc tính DeletedTime để đánh dấu rằng khiếu nại đã bị xóa.
        // Nếu tìm thấy khiếu nại và nó chưa bị xóa, nó sẽ cập nhật thời gian xóa(DeletedTime) và thông tin người thực hiện xóa(DeletedBy), sau đó lưu lại vào database.
        public async Task<bool> DeleteComplaintAsync(Guid id)
        {
            var existingComplaint = await _unitOfWork.GetRepository<Complaint>().GetByIdAsync(id);

            if (existingComplaint == null || existingComplaint.DeletedTime.HasValue)
            {
                return false; // Complaint not found or already deleted
            }

            existingComplaint.DeletedTime = CoreHelper.SystemTimeNow;
            existingComplaint.DeletedBy = "admin"; // Or use context for the user

            _unitOfWork.GetRepository<Complaint>().Update(existingComplaint);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
