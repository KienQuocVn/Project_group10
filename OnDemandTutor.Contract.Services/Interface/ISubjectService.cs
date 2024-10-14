using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.SubjectModelViews;

namespace OnDemandTutor.Contract.Services.Interface;

public interface ISubjectService
{
    Task<BasePaginatedList<Subject>> GetAllSubject(int pageNumber, int pageSize);
    Task<Subject> CreateSubjectAsync(CreateSubjectModelViews model);   
    Task<bool> DeleteSubject(string id); 
    Task<Subject> UpdateSubject(UpdateSubjectModel model);
    Task<BasePaginatedList<Subject>> SearchSubjectsByNameAsync(string subjectName, int pageNumber, int pageSize);
}