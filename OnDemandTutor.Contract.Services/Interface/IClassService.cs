using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.ClassModelViews;
using OnDemandTutor.ModelViews.ScheduleModelViews;


namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IClassService
    {
        Task<BasePaginatedList<Class>> GetAllClassesAsync(int pageNumber, int pageSize, Guid? accountId, string? subjectId, DateTime? startDay, DateTime? endDay);
        Task<Class> GetClassByIdAsync(string id);
        Task<ResponseClassModelView> CreateClassAsync(CreateClassModelView model);
        Task<ResponseClassModelView> UpdateClassAsync(string id, UpdateClassModelView model);
        Task<ResponseClassModelView> DeleteClassAsync(string id);
        Task<Double> CalculateTotalAmount(string id);
    }
}
