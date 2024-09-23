using OnDemandTutor.Core.Base;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface ITutorService
    {
        // Phương thức lấy tất cả môn học của gia sư, sử dụng phân trang  
        // Trả về một danh sách các môn học cho gia sư, có thể phân trang với pageNumber và pageSize  
        Task<BasePaginatedList<TutorSubject>> GetAllTutorSubjectsAsync(int pageNumber, int pageSize);

        // Phương thức lấy một môn học cụ thể theo ID của gia sư và ID môn học  
        // Trả về thông tin môn học của gia sư tương ứng  
        Task<TutorSubject> GetByTutorIdSubjectIdAsync(Guid tutorId, string subjectId);

        // Phương thức tạo một môn học mới cho gia sư  
        // Nhận vào một model chứa thông tin môn học và trả về đối tượng TutorSubject vừa được tạo  
        Task<TutorSubject> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model);

        // Phương thức cập nhật thông tin một môn học của gia sư  
        // Nhận vào ID gia sư, ID môn học và một model cập nhật, trả về thông tin môn học đã cập nhật  
        Task<TutorSubject> UpdateTutorSubjectAsync(Guid tutorId, string subjectId, UpdateTutorSubjectModelViews model);

        // Phương thức xóa môn học của gia sư dựa trên ID gia sư và ID môn học  
        // Trả về trạng thái boolean cho biết việc xóa có thành công hay không  
        Task<bool> DeleteTutorSubjectByTutorIdAndSubjectIdAsync(Guid tutorId, string subjectId);

        // Search 
        Task<BasePaginatedList<TutorSubject>> SearchTutorSubjectsByNameAsync(string subjectName, int pageNumber, int pageSize);
    }
}