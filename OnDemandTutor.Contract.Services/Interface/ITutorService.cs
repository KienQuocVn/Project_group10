using OnDemandTutor.Core.Base;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface ITutorService
    {
        Task<BasePaginatedList<TutorSubject>> GetAllTutor(int pageNumber, int pageSize, Guid? TutorId, Guid? SubjectId);
        Task<BasePaginatedList<TutorSubject>> SearchById(int pageNumber, int pageSize, Guid? TutorId, Guid? SubjectId);
        Task<ResponseTutorModelViews> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model);
        Task<ResponseTutorModelViews> UpdateTutorSubjectAsync(Guid tutorId, Guid subjectId, UpdateTutorSubjectModelViews model);
        Task<ResponseTutorModelViews> DeleteTutorSubjectAsync(Guid tutorId, Guid subjectId);
    }
}