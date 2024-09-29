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
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        protected readonly DatabaseContext _context; // Khai báo context của cơ sở dữ liệu
        protected readonly DbSet<Complaint> _dbSet; // Khai báo DbSet cho Complaint

        public ComplaintRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _context = dbContext; // Khởi tạo context
            _dbSet = _context.Set<Complaint>(); // Khởi tạo DbSet
        }

        // Bạn có thể định nghĩa thêm các phương thức tùy chỉnh nếu cần
    }
}
