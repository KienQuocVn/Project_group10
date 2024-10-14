using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Context;

namespace OnDemandTutor.Repositories.UOW
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public void Delete(object id)
        {
            T entity = _dbSet.Find(id) ?? throw new Exception("Entity not found.");
            _dbSet.Remove(entity);
        }

        public async Task DeleteAsync(object id)
        {
            T entity = await _dbSet.FindAsync(id) ?? throw new Exception("Entity not found.");
            _dbSet.Remove(entity);
        }

        public async Task<T?> FindByNameAsync(string name)
        {
            var propertyInfo = typeof(T).GetProperty("Name");
            if (propertyInfo == null)
            {
                throw new Exception($"Type {typeof(T).Name} does not contain a property named 'Name'.");
            }
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TutorSubject?> GetByTutorIdSubjectIdAsync(Guid tutorId, Guid subjectId)
        {
            if (typeof(T) == typeof(TutorSubject))
            {
                return await _context.TutorSubjects
                    .Where(ts => ts.TutorId == tutorId && ts.SubjectId == subjectId && !ts.DeletedTime.HasValue)
                    .FirstOrDefaultAsync();
            }
            throw new NotImplementedException("This method is only implemented for TutorSubject.");
        }

        public async Task<BasePaginatedList<T>> GetPagging(IQueryable<T> query, int index, int pageSize)
        {
            query = query.AsNoTracking();
            int count = await query.CountAsync();
            var items = await query.Skip((index - 1) * pageSize).Take(pageSize).ToListAsync();
            return new BasePaginatedList<T>(items, count, index, pageSize);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public void InsertRange(IList<T> obj)
        {
            _dbSet.AddRange(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            _dbSet.Entry(obj).State = EntityState.Modified;
        }

        public Task UpdateAsync(T obj)
        {
            _dbSet.Update(obj);
            return Task.CompletedTask;
        }
        public async Task<T?> FindAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
