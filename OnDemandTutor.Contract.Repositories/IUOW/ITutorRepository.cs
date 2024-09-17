using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Repositories.IUOW
{
    public interface ITutorRepository : IGenericRepository<TutorSubject>
    {
        //Task<IEnumerable<TutorSubject>> GetAllAsync();
        //Task<TutorSubject> GetByIdAsync(Guid id);
        //Task AddAsync(TutorSubject tutorSubject);
        //Task UpdateAsync(TutorSubject tutorSubject);
        //Task DeleteAsync(Guid id);
    }
}