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
        Task<TutorSubject> GetByTutorIdSubjectIdAsync(Guid tutorId, string subjectId);
        Task<TutorSubject> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model);
        Task<TutorSubject> UpdateTutorSubjectAsync(Guid tutorId, string subjectId, UpdateTutorSubjectModelViews model);
        Task<bool> DeleteTutorSubjectByTutorIdAndSubjectIdAsync(Guid tutorId, string subjectId);
    }
}