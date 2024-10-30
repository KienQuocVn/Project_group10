using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.RequestRefundModelViews;
using System;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IRequestRefundService
    {
        Task<BasePaginatedList<ResponseRequestRefundModelViews>> GetAllRequestRefundsForAdminAsync(int pageNumber, int pageSize, string? requestId, Guid? accountId, string? status, DateTime? startDate, DateTime? endDate, double? minAmount, double? maxAmount);
        Task<BasePaginatedList<ResponseRequestRefundModelViews>> GetAllRequestRefundsForUsernAsync(int pageNumber, int pageSize, string? status, DateTime? startDate, DateTime? endDate, double? minAmount, double? maxAmount);
        Task<ResponseRequestRefundModelViews> CreateRequestRefundAsync(CreateRequestRefundModelViews model);
        Task<ResponseRequestRefundModelViews> UpdateRequestRefundAsync(string requestId, UpdateRequestRefundModelViews model);
        Task<bool> DeleteRequestRefundAsync(string requestId);
    }
}
