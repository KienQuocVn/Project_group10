using OnDemandTutor.Core.Base;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface ITutorService
    {
        Task<BasePaginatedList<TutorSubject>> GetAllTutor(int pageNumber, int pageSize, Guid? TutorId, string? SubjectId);
        Task<BasePaginatedList<TutorSubject>> SearchById(int pageNumber, int pageSize, Guid? TutorId, string? SubjectId);
        Task<ResponseTutorModelViews> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model);
        Task<ResponseTutorModelViews> UpdateTutorSubjectAsync(Guid tutorId, string subjectId, UpdateTutorSubjectModelViews model);
        Task<ResponseTutorModelViews> DeleteTutorSubjectAsync(Guid tutorId, string subjectId);
    }
}