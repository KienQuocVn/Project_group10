using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Core.Utils;
using OnDemandTutor.ModelViews.SubjectModelViews;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Services.Service;

public class SubjectService : ISubjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BasePaginatedList<Subject>> GetAllSubject(int pageNumber, int pageSize)
    {
        // L?y danh sách môn h?c ch?a b? xóa và s?p x?p theo th?i gian t?o gi?m d?n  
        IQueryable<Subject> SubjectsQuery = _unitOfWork.GetRepository<Subject>()
            .Entities
            .Where(p => !p.DeletedTime.HasValue)
            .OrderByDescending(p => p.CreatedTime);

        // ??m t?ng s? môn h?c không b? xóa  
        int totalCount = await SubjectsQuery.CountAsync();

        // L?y danh sách môn h?c theo trang ?ã ch? ??nh  
        var subjects = await SubjectsQuery
            .OrderBy(s => s.Id)  // S?p x?p theo Id  
            .Skip((pageNumber - 1) * pageSize) // B? qua s? l??ng môn h?c c?a các trang tr??c  
            .Take(pageSize) // L?y s? l??ng môn h?c theo kích th??c trang  
            .ToListAsync();

        // Tr? v? danh sách môn h?c ?ã phân trang  
        return new BasePaginatedList<Subject>(subjects, totalCount, pageNumber, pageSize);
    }

    public async Task<Subject> CreateSubjectAsync(CreateSubjectModelViews model)
    {
        // Ki?m tra tính h?p l? c?a mô hình  
        if (model == null || string.IsNullOrEmpty(model.Name))
        {
            Console.WriteLine("D? li?u môn h?c không h?p l?.");
            throw new ArgumentException("D? li?u môn h?c không h?p l?.");
        }

        try
        {
            // T?o ??i t??ng môn h?c m?i t? mô hình  
            var subjectEntity = new Subject
            {
                Id = model.Id ?? Guid.NewGuid().ToString(), // T?o ID m?i n?u không có  
                Name = model.Name,
                CreatedBy = model.CreatedBy,
                CreatedTime = DateTimeOffset.Now,
                LastUpdatedTime = DateTimeOffset.Now
            };

            // Thêm môn h?c vào c? s? d? li?u  
            await _unitOfWork.SubjectRepository.InsertAsync(subjectEntity);
            await _unitOfWork.SaveAsync(); // L?u thay ??i vào c? s? d? li?u  

            // In ra thông báo thành công  
            Console.WriteLine($"T?o m?i môn h?c thành công: Id = {subjectEntity.Id}");

            // Tr? v? môn h?c ?ã t?o  
            return subjectEntity;
        }
        catch (Exception ex)
        {
            // In ra thông báo l?i n?u có  
            Console.WriteLine($"L?i khi t?o m?i môn h?c: {ex.Message}");
            throw new Exception("Không th? t?o m?i môn h?c", ex); // Ném ra l?i ?? x? lý ? n?i khác  
        }
    }


    public async Task<bool> DeleteSubject(string id)
    {
        // L?y môn h?c hi?n t?i b?ng ID  
        var existingSubject = await _unitOfWork.GetRepository<Subject>().GetByIdAsync(id);

        // Ki?m tra xem môn h?c có t?n t?i không  
        if (existingSubject == null)
        {
            Console.WriteLine($"Không tìm th?y môn h?c v?i ID: {id}");
            return false; // Môn h?c không t?n t?i  
        }

        // ?ánh d?u môn h?c là ?ã xóa b?ng cách thi?t l?p DeletedTime và DeletedBy  
        existingSubject.DeletedTime = CoreHelper.SystemTimeNow; // Thi?t l?p th?i gian xóa  
        existingSubject.DeletedBy = "admin"; // Có th? thay th? b?ng thông tin ng??i dùng t? ng? c?nh  

        // C?p nh?t môn h?c trong c? s? d? li?u  
        _unitOfWork.GetRepository<Subject>().Update(existingSubject);
        await _unitOfWork.SaveAsync(); // L?u thay ??i vào c? s? d? li?u  

        // In ra thông báo thành công  
        Console.WriteLine($"Môn h?c v?i ID: {id} ?ã ???c xóa thành công.");
        return true;
    }


    public async Task<Subject> UpdateSubject(UpdateSubjectModel model)
    {
        // L?y môn h?c hi?n t?i b?ng ID t? mô hình c?p nh?t  
        var existingSubject = await _unitOfWork.GetRepository<Subject>().GetByIdAsync(model.Id);

        // Ki?m tra xem môn h?c có t?n t?i không  
        if (existingSubject == null)
        {
            Console.WriteLine($"Không tìm th?y môn h?c v?i ID: {model.Id}");
            return null; // Môn h?c không t?n t?i  
        }

        // C?p nh?t các thu?c tính c?a môn h?c hi?n t?i  
        existingSubject.Name = model.Name; // C?p nh?t tên môn h?c  
        existingSubject.LastUpdatedBy = model.UpdateBy; // C?p nh?t ng??i th?c hi?n c?p nh?t  
        existingSubject.LastUpdatedTime = CoreHelper.SystemTimeNow; // C?p nh?t th?i gian c?p nh?t  

        // C?p nh?t môn h?c trong c? s? d? li?u  
        _unitOfWork.GetRepository<Subject>().Update(existingSubject);
        await _unitOfWork.SaveAsync(); // L?u thay ??i vào c? s? d? li?u  

        // In ra thông báo thành công  
        Console.WriteLine($"Môn h?c v?i ID: {model.Id} ?ã ???c c?p nh?t thành công.");
        return existingSubject; // Tr? v? môn h?c ?ã c?p nh?t  
    }


    public async Task<BasePaginatedList<Subject>> SearchSubjectsByNameAsync(string subjectName, int pageNumber, int pageSize)
    {
        // L?y danh sách Subject có tên ch?a subjectName và phân trang  
        var query = _unitOfWork.SubjectRepository.Entities
            .Where(s => s.Name.Contains(subjectName));

        // L?y t?ng s? Subject th?a mãn ?i?u ki?n  
        var totalCount = await query.CountAsync();

        if (totalCount == 0)
        {
            return new BasePaginatedList<Subject>(new List<Subject>(), totalCount, pageNumber, pageSize);
        }

        // Th?c hi?n phân trang  
        var subjects = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new BasePaginatedList<Subject>(subjects, totalCount, pageNumber, pageSize);
    }
}