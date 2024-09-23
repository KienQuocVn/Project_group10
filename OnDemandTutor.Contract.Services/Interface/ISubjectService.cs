using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews;

namespace OnDemandTutor.Contract.Services.Interface;

public interface ISubjectService 
{
    Task<BasePaginatedList<Subject>> GetAllSubject(int pageNumber, int pageSize);
    Task AddSubject(string subjectName);
    Task<bool> DeleteSubject(string id);
    Task<Subject>  UpdateSubject(UpdateSubjectModel model);
}