using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Core.Utils;
using OnDemandTutor.ModelViews.SubjectModelViews;

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
        IQueryable<Subject> SubjectsQuery = _unitOfWork.GetRepository<Subject>().Entities.Where(p => !p.DeletedTime.HasValue).OrderByDescending(p => p.CreatedTime);
        int totalCount = await SubjectsQuery.CountAsync();
        var subject = await SubjectsQuery
            .OrderBy(s => s.Id)  
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new BasePaginatedList<Subject>(subject, totalCount, pageNumber, pageSize);
    }

    public async Task AddSubject(string subject)
    {
        var subjectEntity = new Subject
        {
            Name = subject,
            TutorSubjects = new List<TutorSubject>(),
            Classes = new List<Class>()
        };
        await _unitOfWork.GetRepository<Subject>().InsertAsync(subjectEntity);
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteSubject(string id)
    {
        var existingSubject = await _unitOfWork.GetRepository<Subject>().GetByIdAsync(id);

        if (existingSubject == null)
        {
            return false; // Subject not found
        }

        // Set DeletedTime to the current time and optionally set DeletedBy
        existingSubject.DeletedTime = CoreHelper.SystemTimeNow;
        existingSubject.DeletedBy = "admin"; // or use a user identity from the context

        _unitOfWork.GetRepository<Subject>().Update(existingSubject);
        await _unitOfWork.SaveAsync(); // Save changes

        return true;
        
    }

    public async Task<Subject> UpdateSubject(UpdateSubjectModel model)
    {
        var existingSubject = await _unitOfWork.GetRepository<Subject>().GetByIdAsync(model.Id);

        if (existingSubject == null)
        {
            //throw new KeyNotFoundException($"Subject with Id '{model}' not found.");
            return null;
        }

        // Update the properties of the existing subject
        existingSubject.Name = model.Name;
        existingSubject.LastUpdatedBy = model.UpdateBy;
        existingSubject.LastUpdatedTime = CoreHelper.SystemTimeNow;

        _unitOfWork.GetRepository<Subject>().UpdateAsync(existingSubject);
        await _unitOfWork.SaveAsync();  // Save changes

        return existingSubject;
    }
}