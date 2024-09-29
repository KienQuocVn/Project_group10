using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ComplaintModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IUnitOfWork _unitOfWork; // Đơn vị công việc để xử lý các thao tác với cơ sở dữ liệu
        private readonly IMapper _mapper; // Mapper để ánh xạ giữa các đối tượng

        public ComplaintService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; // Khởi tạo đơn vị công việc
            _mapper = mapper; // Khởi tạo mapper
        }

        // Lấy tất cả khiếu nại với phân trang
        public async Task<BasePaginatedList<Complaint>> GetAllComplaintsAsync(int pageNumber, int pageSize, Guid? studentId, Guid? tutorId, string? status)
        {
            // Lấy truy vấn các khiếu nại và sắp xếp theo thời gian tạo giảm dần
            IQueryable<Complaint> complaintsQuery = _unitOfWork.GetRepository<Complaint>().Entities
                .OrderByDescending(c => c.CreatedAt);

            // Lọc theo ID sinh viên nếu có
            if (studentId.HasValue)
            {
                complaintsQuery = complaintsQuery.Where(c => c.StudentId == studentId);
            }

            // Lọc theo ID gia sư nếu có
            if (tutorId.HasValue)
            {
                complaintsQuery = complaintsQuery.Where(c => c.TutorId == tutorId);
            }

            // Lọc theo trạng thái nếu có
            if (!string.IsNullOrWhiteSpace(status))
            {
                complaintsQuery = complaintsQuery.Where(c => c.Status == status);
            }

            // Tính tổng số khiếu nại
            int totalCount = await complaintsQuery.CountAsync();
            // Lấy danh sách khiếu nại theo phân trang
            List<Complaint> paginatedComplaints = await complaintsQuery
                .Skip((pageNumber - 1) * pageSize) // Bỏ qua số lượng khiếu nại ở các trang trước
                .Take(pageSize) // Lấy số lượng khiếu nại theo kích thước trang
                .ToListAsync();

            // Trả về danh sách khiếu nại đã phân trang
            return new BasePaginatedList<Complaint>(paginatedComplaints, totalCount, pageNumber, pageSize);
        }

        // Lấy một khiếu nại theo ID
        public async Task<ResponseComplaintModel> GetComplaintByIdAsync(Guid id)
        {
            // Chuyển đổi id từ Guid sang string
            var complaint = await _unitOfWork.GetRepository<Complaint>().Entities
                .FirstOrDefaultAsync(c => c.Id == id.ToString()) // So sánh với string
                ?? throw new Exception("Complaint not found."); // Ném ra ngoại lệ nếu không tìm thấy

            // Ánh xạ khiếu nại thành ResponseComplaintModel
            return _mapper.Map<ResponseComplaintModel>(complaint);
        }

        // Tạo một khiếu nại mới
        public async Task<ResponseComplaintModel> CreateComplaintAsync(CreateComplaintModel model)
        {
            // Ánh xạ từ CreateComplaintModel sang Complaint
            var complaint = _mapper.Map<Complaint>(model);

            // Tạo ID mới cho khiếu nại và chuyển đổi sang string
            complaint.Id = Guid.NewGuid().ToString("N"); // Chuyển Guid thành string

            // Thêm khiếu nại vào cơ sở dữ liệu
            await _unitOfWork.GetRepository<Complaint>().InsertAsync(complaint);

            // Lưu thay đổi
            await _unitOfWork.SaveAsync();

            // Trả về ResponseComplaintModel
            return _mapper.Map<ResponseComplaintModel>(complaint);
        }


        // Cập nhật một khiếu nại theo ID
        public async Task<ResponseComplaintModel> UpdateComplaintAsync(Guid id, UpdateComplaintModel model)
        {
            // Chuyển đổi Guid thành string để so sánh
            var idString = id.ToString("N");

            // Tìm khiếu nại theo ID, nếu không tìm thấy thì ném ra ngoại lệ
            var existingComplaint = await _unitOfWork.GetRepository<Complaint>().Entities
                .FirstOrDefaultAsync(c => c.Id == idString) ?? throw new Exception("Complaint not found.");

            // Ánh xạ các trường từ model vào khiếu nại đã tồn tại
            _mapper.Map(model, existingComplaint);

            // Lưu thay đổi
            await _unitOfWork.SaveAsync();

            // Trả về ResponseComplaintModel
            return _mapper.Map<ResponseComplaintModel>(existingComplaint);
        }


        // Xóa một khiếu nại theo ID
        public async Task<ResponseComplaintModel> DeleteComplaintAsync(Guid id)
        {
            // Chuyển đổi Guid thành string để so sánh
            var idString = id.ToString("N");

            // Tìm khiếu nại theo ID, nếu không tìm thấy thì ném ra ngoại lệ
            var existingComplaint = await _unitOfWork.GetRepository<Complaint>().Entities
                .FirstOrDefaultAsync(c => c.Id == idString) ?? throw new Exception("Complaint not found.");

            // Xóa khiếu nại
            _unitOfWork.GetRepository<Complaint>().Delete(existingComplaint);

            // Lưu thay đổi
            await _unitOfWork.SaveAsync();

            // Trả về ResponseComplaintModel
            return _mapper.Map<ResponseComplaintModel>(existingComplaint);
        }

    }
}
