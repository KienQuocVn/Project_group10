using OnDemandTutor.Contract.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        IGenericRepository<Schedule> ScheduleRepository { get; }

        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();

    }
}
