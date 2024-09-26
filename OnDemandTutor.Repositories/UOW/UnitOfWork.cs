using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;

namespace OnDemandTutor.Repositories.UOW
{
    public class UnitOfWork(DatabaseContext dbContext) : IUnitOfWork
    {
        private bool disposed = false;
        private readonly DatabaseContext _dbContext = dbContext;

        public IGenericRepository<Schedule> scheduleRepository;

        public IGenericRepository<Schedule> ScheduleRepository
        {
            get
            {
                if (this.scheduleRepository == null)
                {
                    this.scheduleRepository = new GenericRepository<Schedule>(_dbContext);
                }
                return scheduleRepository;
            }
        }

        public IGenericRepository<TutorSubject> tutorRepository;

        public IGenericRepository<TutorSubject> TutorRepository
        {
            get
            {
                if (this.tutorRepository == null)
                {
                    this.tutorRepository = new GenericRepository<TutorSubject>(_dbContext);
                }
                return tutorRepository;
            }
        }


        public IGenericRepository<Feedback> feedbackRepository;

        public IGenericRepository<Feedback> FeedbackRepository
        {
            get
            {
                if (this.feedbackRepository == null)
                {
                    this.feedbackRepository = new GenericRepository<Feedback>(_dbContext);
                }
                return feedbackRepository;
            }
        }

        
        public IGenericRepository<Subject> subjectRepository;
        public IGenericRepository<Subject> SubjectRepository
        {
            get
            {
                if (this.subjectRepository == null)
                {
                    this.subjectRepository = new GenericRepository<Subject>(_dbContext);
                }
                return subjectRepository;
            }
        }

        public IGenericRepository<Class> classRepository;
        public IGenericRepository<Class> ClassRepository
        {
            get
            {
                if (this.classRepository == null)
                {
                    this.classRepository = new GenericRepository<Class>(_dbContext);
                }
                return classRepository;
            }
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void RollBack()
        {
            _dbContext.Database.RollbackTransaction();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }

       
    }
}
