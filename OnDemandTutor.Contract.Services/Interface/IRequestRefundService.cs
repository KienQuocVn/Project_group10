using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.RequestRefundModelViews;


namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IRequestRefundService
    {
        Task<BasePaginatedList<ResponseRequestRefundModelViews>> GetAllRequestRefundsAsync(int pageNumber, int pageSize, string? requestId, Guid? accountId, string? status, DateTime? startDate, DateTime? endDate);
        Task<ResponseRequestRefundModelViews> CreateRequestRefundAsync(CreateRequestRefundModelViews model);
    }
}
