using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;
using OnDemandTutor.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Repositories.UOW
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<Feedback> _dbSet;
        public FeedbackRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<Feedback>();
        }


    }
}
