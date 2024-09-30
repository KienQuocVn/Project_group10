using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews;

namespace OnDemandTutor.Contract.Services.Interface;

// ??nh ngh?a m?t giao di?n cho d?ch v? qu?n lý các ??i t??ng Subject  
public interface ISubjectService
{
    // Ph??ng th?c ?? l?y t?t c? các Subject v?i phân trang  
    // pageNumber: s? trang (th? t? t? 1)  
    // pageSize: s? l??ng Subject trên m?i trang  
    // Tr? v? m?t danh sách các Subject ???c phân trang  
    Task<BasePaginatedList<Subject>> GetAllSubject(int pageNumber, int pageSize);

    // Ph??ng th?c ?? t?o m?i m?t Subject  
    // model: ??i t??ng ch?a thông tin c?n thi?t ?? t?o Subject  
    // Tr? v? Subject m?i ?ã ???c t?o  
    Task<Subject> CreateSubjectAsync(CreateSubjectModelViews model); // C?p nh?t ph??ng th?c  

    // Ph??ng th?c ?? xóa m?t Subject d?a trên ID  
    // id: ??nh danh c?a Subject c?n xóa  
    // Tr? v? true n?u xóa thành công, false n?u không  
    Task<bool> DeleteSubject(string id);

    // Ph??ng th?c ?? c?p nh?t thông tin c?a m?t Subject  
    // model: ??i t??ng ch?a thông tin c?n c?p nh?t  
    // Tr? v? Subject ?ã ???c c?p nh?t  
    Task<Subject> UpdateSubject(UpdateSubjectModel model);


    Task<BasePaginatedList<Subject>> SearchSubjectsByNameAsync(string subjectName, int pageNumber, int pageSize);
}