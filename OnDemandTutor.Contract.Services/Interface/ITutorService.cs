using OnDemandTutor.Core.Base;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface ITutorService
    {
        Task<BasePaginatedList<TutorSubject>> GetAllTutorSubjectsAsync(int pageNumber, int pageSize);
        Task<BasePaginatedList<TutorSubject>> SearchTutorSubjectsAsync(int pageNumber, int pageSize, Guid? studentId = null, string? slotId = null);
        Task<TutorSubject> GetTutorSubjectByIdAsync(Guid id);
        Task<TutorSubject> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model);
        Task<TutorSubject> UpdateTutorSubjectAsync(Guid id, UpdateTutorSubjectModelViews model);
        Task<bool> DeleteTutorSubjectAsync(Guid id);
    }
}