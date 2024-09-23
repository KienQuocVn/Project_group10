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
        // Khai báo biến để lưu trữ ngữ cảnh cơ sở dữ liệu  
        protected readonly DatabaseContext _context;

        // Khai báo biến để lưu trữ tập hợp TutorSubject trong cơ sở dữ liệu  
        protected readonly DbSet<TutorSubject> _dbSet;

        // Constructor nhận vào DatabaseContext và truyền cho lớp cơ sở thông qua base  
        public TutorRepository(DatabaseContext dbContext) : base(dbContext)
        {
            // Gán ngữ cảnh cơ sở dữ liệu cho biến _context  
            _context = dbContext;

            // Thiết lập _dbSet để làm việc với các thực thể TutorSubject  
            _dbSet = _context.Set<TutorSubject>();
        }


    }
}