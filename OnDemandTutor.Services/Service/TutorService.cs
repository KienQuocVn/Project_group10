using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;

using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Repositories.UOW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnDemandTutor.Services.Service
{
    public class TutorService : ITutorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TutorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BasePaginatedList<TutorSubject>> GetAllTutorSubjectsAsync(int pageNumber, int pageSize)
        {
            var tutorSubjectQuery = _unitOfWork.GetRepository<TutorSubject>().Entities
                .Where(p => !p.DeletedTime.HasValue)
                .OrderByDescending(p => p.CreatedTime);

            int totalCount = await tutorSubjectQuery.CountAsync();
            var tutorSubjects = await tutorSubjectQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<TutorSubject>(tutorSubjects, totalCount, pageNumber, pageSize);
        }

        public async Task<TutorSubject> GetTutorSubjectByIdAsync(Guid id)
        {
            return await _unitOfWork.TutorRepository.GetByIdAsync(id);
        }

        public async Task<BasePaginatedList<TutorSubject>> SearchTutorSubjectsAsync(int pageNumber, int pageSize, Guid? TutorId = null, string? SubjectId = null)
        {
            var tutorSubjectQuery = _unitOfWork.GetRepository<TutorSubject>().Entities
                .Include(p => p.Tutor)
                .Include(p => p.Subject)
                .Where(p => !p.DeletedTime.HasValue)
                .Where(p => (!TutorId.HasValue || p.Tutor.Id == TutorId) &&
                            (string.IsNullOrEmpty(SubjectId) || p.Subject.Id == SubjectId))
                .OrderByDescending(p => p.Id);

            int totalCount = await tutorSubjectQuery.CountAsync();
            var tutorSubjects = await tutorSubjectQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<TutorSubject>(tutorSubjects, totalCount, pageNumber, pageSize);
        }

        public async Task<TutorSubject> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model)
        {
            var tutorSubject = new TutorSubject
            {
                Id = Guid.NewGuid(),
                TutorId = model.TutorId,
                CreatedTime = DateTimeOffset.Now,
                LastUpdatedTime = DateTimeOffset.Now
            };
            await _unitOfWork.TutorRepository.InsertAsync(tutorSubject);
            await _unitOfWork.SaveAsync();

            return tutorSubject;
        }

        public async Task<TutorSubject> UpdateTutorSubjectAsync(Guid id, UpdateTutorSubjectModelViews model)
        {
            var existingTutorSubject = await _unitOfWork.TutorRepository.GetByIdAsync(id);
            if (existingTutorSubject != null)
            {
                existingTutorSubject.LastUpdatedTime = DateTimeOffset.Now;
                _unitOfWork.TutorRepository.Update(existingTutorSubject);
                await _unitOfWork.SaveAsync();
                return existingTutorSubject;
            }
            return null;
        }

        public async Task<bool> DeleteTutorSubjectAsync(Guid id)
        {
            var existingTutorSubject = await _unitOfWork.TutorRepository.GetByIdAsync(id);
            if (existingTutorSubject != null)
            {
                existingTutorSubject.DeletedTime = DateTimeOffset.Now; // Soft delete  
                _unitOfWork.TutorRepository.Update(existingTutorSubject);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }
    }
}