using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using System;

namespace OnDemandTutor.Contract.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class; 
        IGenericRepository<Schedule> ScheduleRepository { get; }
<<<<<<< HEAD
        IGenericRepository<TutorSubject> TutorRepository { get; }
        IGenericRepository<Subject> SubjectRepository { get; }
        IGenericRepository<Feedback> FeedbackRepository { get; }
        IGenericRepository<Slot> SlotRepository { get; }
=======
        IGenericRepository<TutorSubject> TutorRepository { get; } 
        IGenericRepository<Subject> SubjectRepository { get; } 
        IGenericRepository<Feedback> FeedbackRepository { get; } 
        IGenericRepository<Complaint> ComplaintRepository { get; } 
        IGenericRepository<Class> ClassRepository { get; }

>>>>>>> 736f99d09baea832d78df3e5777752264735af48

        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
