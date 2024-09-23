using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ScheduleModelViews;


namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IScheduleService
    {
        Task<BasePaginatedList<Schedule>> GetAllSchedulesAsync(int pageNumber, int pageSize, Guid? studentId, string? slotId, String? status);

        Task<BasePaginatedList<Schedule>> GetSchedulesByFilterAsync(int pageNumber, int pageSize, Guid? studentId, string? slotId, String? status);

        Task<ResponseScheduleModelViews> GetScheduleByIdAsync(string id);
        Task<ResponseScheduleModelViews> CreateScheduleAsync(CreateScheduleModelViews model);
        Task<ResponseScheduleModelViews> UpdateScheduleAsync(String id, UpdateScheduleModelViews schedule);
        Task<ResponseScheduleModelViews> DeleteScheduleAsync(string id);
    }
}
