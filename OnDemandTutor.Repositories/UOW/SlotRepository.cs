using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Repositories.UOW
{
    public class SlotRepository : GenericRepository<Slot> , ISlotRepository
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<Feedback> _dbSet;
        public SlotRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<Feedback>();
        }
    }
}
