using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;


namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IScheduleService
    {
        Task<BasePaginatedList<Schedule>> GetAllSchedulesAsync(int pageNumber, int pageSize, Guid? studentId, string? slotId, String? status);
        Task<BasePaginatedList<Schedule>> GetSchedulesByFilterAsync(int pageNumber, int pageSize, Guid? studentId, string? slotId, String? status);
        Task<ResponseScheduleModelViews> CreateScheduleAsync(CreateScheduleModelViews model);
        Task<ResponseScheduleModelViews> UpdateScheduleAsync(Guid studentId, string slotId, UpdateScheduleModelViews schedule);
        Task<ResponseScheduleModelViews> DeleteScheduleAsync(Guid studentId, string slotId);
    }
}
