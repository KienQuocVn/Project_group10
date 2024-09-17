using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTutor.Repositories.UOW
{
    public class TutorRepository : GenericRepository<TutorSubject>, ITutorRepository
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<Schedule> _dbSet;
        public TutorRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<Schedule>();
        }
    }
}