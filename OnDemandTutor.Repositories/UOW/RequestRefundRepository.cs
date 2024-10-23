using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Repositories.UOW
{
    public class RequestRefundRepository : GenericRepository<RequestRefund>, IRequestRedunfRepository
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<RequestRefund> _dbSet;
        public RequestRefundRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<RequestRefund>();
        }
    }
}
