using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;
using System;

namespace OnDemandTutor.Repositories.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false; // Biến để kiểm tra xem đối tượng đã được giải phóng hay chưa
        private readonly DatabaseContext _dbContext; // Bối cảnh cơ sở dữ liệu

        public UnitOfWork(DatabaseContext dbContext) // Constructor
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<Schedule> _scheduleRepository; // Repository cho Schedule
        public IGenericRepository<Schedule> ScheduleRepository
        {
            get
            {
                if (_scheduleRepository == null)
                {
                    _scheduleRepository = new GenericRepository<Schedule>(_dbContext);
                }
                return _scheduleRepository;
            }
        }

        private IGenericRepository<TutorSubject> _tutorRepository; // Repository cho TutorSubject
        public IGenericRepository<TutorSubject> TutorRepository
        {
            get
            {
                if (_tutorRepository == null)
                {
                    _tutorRepository = new GenericRepository<TutorSubject>(_dbContext);
                }
                return _tutorRepository;
            }
        }

        private IGenericRepository<Feedback> _feedbackRepository; // Repository cho Feedback
        public IGenericRepository<Feedback> FeedbackRepository
        {
            get
            {
                if (_feedbackRepository == null)
                {
                    _feedbackRepository = new GenericRepository<Feedback>(_dbContext);
                }
                return _feedbackRepository;
            }
        }

        private IGenericRepository<Subject> _subjectRepository; // Repository cho Subject
        public IGenericRepository<Subject> SubjectRepository
        {
            get
            {
                if (_subjectRepository == null)
                {
                    _subjectRepository = new GenericRepository<Subject>(_dbContext);
                }
                return _subjectRepository;
            }
        }

        private IGenericRepository<Complaint> _complaintRepository; // Repository cho Complaint
        public IGenericRepository<Complaint> ComplaintRepository
        {
            get
            {
                if (_complaintRepository == null)
                {
                    _complaintRepository = new GenericRepository<Complaint>(_dbContext);
                }
                return _complaintRepository;
            }
        }

        public void BeginTransaction() // Bắt đầu giao dịch
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction() // Cam kết giao dịch
        {
            _dbContext.Database.CommitTransaction();
        }

        public void Dispose() // Giải phóng tài nguyên
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) // Phương thức giải phóng tài nguyên
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

        public void RollBack() // Hoàn tác giao dịch
        {
            _dbContext.Database.RollbackTransaction();
        }

        public void Save() // Lưu thay đổi
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync() // Lưu thay đổi không đồng bộ
        {
            await _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class // Lấy repository cho loại thực thể cụ thể
        {
            return new GenericRepository<T>(_dbContext);
        }
    }
}
