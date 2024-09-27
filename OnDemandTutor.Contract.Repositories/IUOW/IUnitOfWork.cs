using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using System;

namespace OnDemandTutor.Contract.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class; // Phương thức lấy repository chung cho mọi loại thực thể
        IGenericRepository<Schedule> ScheduleRepository { get; } // Repository cho Schedule
        IGenericRepository<TutorSubject> TutorRepository { get; } // Repository cho TutorSubject
        IGenericRepository<Subject> SubjectRepository { get; } // Repository cho Subject
        IGenericRepository<Feedback> FeedbackRepository { get; } // Repository cho Feedback
        IGenericRepository<Complaint> ComplaintRepository { get; } // Đã sửa từ ComplaintRpository sang ComplaintRepository
        IGenericRepository<T> GetRepository<T>() where T : class;
        IGenericRepository<Schedule> ScheduleRepository { get; }
        IGenericRepository<TutorSubject> TutorRepository { get; }
        IGenericRepository<Subject> SubjectRepository { get; }
        IGenericRepository<Feedback> FeedbackRepository { get; }
        IGenericRepository<Class> ClassRepository { get; }


        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();

        void Save(); // Lưu thay đổi
        Task SaveAsync(); // Lưu thay đổi không đồng bộ
        void BeginTransaction(); // Bắt đầu giao dịch
        void CommitTransaction(); // Cam kết giao dịch
        void RollBack(); // Hoàn tác giao dịch
    }
}
