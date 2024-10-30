using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.RequestRefundModelViews;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Services.Service.AccountUltil;

namespace OnDemandTutor.Services.Service
{
    public class RequestRefundService : IRequestRefundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountUtils _accountUtil;
        private readonly IClassService _classService;
        private readonly IMapper _mapper;

        public RequestRefundService(IUnitOfWork unitOfWork, IMapper mapper, IClassService classService, AccountUtils accountUtil)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _classService = classService;
            _accountUtil = accountUtil;
        }

        // Lấy danh sách các yêu cầu hoàn tiền với điều kiện tìm kiếm và phân trang cho admin
        public async Task<BasePaginatedList<ResponseRequestRefundModelViews>> GetAllRequestRefundsForAdminAsync(int pageNumber, int pageSize, string? requestId, Guid? accountId, string? status, DateTime? startDate, DateTime? endDate, double? minAmount, double? maxAmount)
        {
            IQueryable<RequestRefund> requestRefundsQuery = _unitOfWork.GetRepository<RequestRefund>().Entities
                .Where(p => !p.DeletedTime.HasValue || string.IsNullOrEmpty(p.DeletedBy))
                .OrderByDescending(r => r.CreatedTime);

            if (!string.IsNullOrWhiteSpace(requestId))
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Id == requestId);
            }

            if (accountId.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.AccountId == accountId);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Status == status);
            }

            if (startDate.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.CreatedTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.CreatedTime <= endDate.Value);
            }

            if (minAmount.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Amount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Amount <= maxAmount.Value);
            }

            int totalCount = await requestRefundsQuery.CountAsync();

            requestRefundsQuery = requestRefundsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            List<RequestRefund> paginatedRequestRefunds = await requestRefundsQuery.ToListAsync();

            var response = _mapper.Map<List<ResponseRequestRefundModelViews>>(paginatedRequestRefunds);

            return new BasePaginatedList<ResponseRequestRefundModelViews>(response, totalCount, pageNumber, pageSize);
        }

        // Lấy danh sách các yêu cầu hoàn tiền cho người dùng hiện tại
        public async Task<BasePaginatedList<ResponseRequestRefundModelViews>> GetAllRequestRefundsForUsernAsync(int pageNumber, int pageSize, string? status, DateTime? startDate, DateTime? endDate, double? minAmount, double? maxAmount)
        {
            IQueryable<RequestRefund> requestRefundsQuery = _unitOfWork.GetRepository<RequestRefund>().Entities
                .Where(p => !p.DeletedTime.HasValue || string.IsNullOrEmpty(p.DeletedBy))
                .OrderByDescending(r => r.CreatedTime);

            Accounts account = _accountUtil.GetCurrentUser();
            if (account == null)
            {
                throw new Exception("Unable to determine the current user account.");
            }

            requestRefundsQuery = requestRefundsQuery.Where(r => r.AccountId == account.Id);

            if (!string.IsNullOrWhiteSpace(status))
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Status == status);
            }

            if (startDate.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.CreatedTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.CreatedTime <= endDate.Value);
            }

            if (minAmount.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Amount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Amount <= maxAmount.Value);
            }

            int totalCount = await requestRefundsQuery.CountAsync();

            requestRefundsQuery = requestRefundsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            List<RequestRefund> paginatedRequestRefunds = await requestRefundsQuery.ToListAsync();

            var response = _mapper.Map<List<ResponseRequestRefundModelViews>>(paginatedRequestRefunds);

            return new BasePaginatedList<ResponseRequestRefundModelViews>(response, totalCount, pageNumber, pageSize);
        }

        // Tạo một yêu cầu hoàn tiền mới
        public async Task<ResponseRequestRefundModelViews> CreateRequestRefundAsync(CreateRequestRefundModelViews model)
        {
            if (model.Amount <= 0)
            {
                throw new Exception("Refund amount must be greater than 0.");
            }

            Accounts account = _accountUtil.GetCurrentUser();
            if (account == null)
            {
                throw new Exception("Unable to determine the current user account.");
            }

            UserInfo user = await _unitOfWork.GetRepository<UserInfo>().GetByIdAsync(account.UserInfo.Id);
            if (user == null)
            {
                throw new Exception("User Info cannot be found or has been deleted!");
            }

            if (user.Balance < model.Amount)
            {
                throw new Exception("Account balance is insufficient to make a refund request.");
            }

            RequestRefund newRequestRefund = _mapper.Map<RequestRefund>(model);
            newRequestRefund.Id = Guid.NewGuid().ToString("N");
            newRequestRefund.Status = "Wait";
            newRequestRefund.CreatedBy = account.Id.ToString();
            newRequestRefund.CreatedTime = DateTimeOffset.UtcNow;
            newRequestRefund.LastUpdatedTime = DateTimeOffset.UtcNow;

            await _unitOfWork.GetRepository<RequestRefund>().InsertAsync(newRequestRefund);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ResponseRequestRefundModelViews>(newRequestRefund);
        }


        public async Task<ResponseRequestRefundModelViews> UpdateRequestRefundAsync(string requestId, UpdateRequestRefundModelViews model)
        {
            var requestRefund = await _unitOfWork.GetRepository<RequestRefund>().GetByIdAsync(requestId);

            if (requestRefund == null || requestRefund.DeletedTime.HasValue)
            {
                throw new Exception("Refund request not found or has been deleted.");
            }
            if (string.IsNullOrWhiteSpace(model.Status))
            {
                throw new Exception("Please enter a status.");
            }

            // Áp dụng các thay đổi từ model vào đối tượng requestRefund
            requestRefund = _mapper.Map(model, requestRefund);
            requestRefund.LastUpdatedTime = DateTimeOffset.UtcNow;

            await _unitOfWork.GetRepository<RequestRefund>().UpdateAsync(requestRefund);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ResponseRequestRefundModelViews>(requestRefund);
        }

        // Xóa mềm yêu cầu hoàn tiền
        public async Task<bool> DeleteRequestRefundAsync(string requestId)
        {
            var requestRefund = await _unitOfWork.GetRepository<RequestRefund>().GetByIdAsync(requestId);

            if (requestRefund == null || requestRefund.DeletedTime.HasValue)
            {
                throw new Exception("Refund request not found or has already been deleted.");
            }

            // Đánh dấu xóa mềm
            requestRefund.DeletedTime = DateTimeOffset.UtcNow;
            requestRefund.DeletedBy = _accountUtil.GetCurrentUser()?.Id.ToString();

            await _unitOfWork.GetRepository<RequestRefund>().UpdateAsync(requestRefund);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
