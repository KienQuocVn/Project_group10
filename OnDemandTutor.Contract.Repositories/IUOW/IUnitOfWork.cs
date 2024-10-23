﻿using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using System;

namespace OnDemandTutor.Contract.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class; 
        IGenericRepository<Schedule> ScheduleRepository { get; }
        IGenericRepository<TutorSubject> TutorRepository { get; }
        IGenericRepository<Subject> SubjectRepository { get; }
        IGenericRepository<Feedback> FeedbackRepository { get; }
        IGenericRepository<Slot> SlotRepository { get; }
        IGenericRepository<Complaint> ComplaintRepository { get; } 
        IGenericRepository<Class> ClassRepository { get; }
        IGenericRepository<RequestRefund> RequestRefundRepository { get; }

        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
