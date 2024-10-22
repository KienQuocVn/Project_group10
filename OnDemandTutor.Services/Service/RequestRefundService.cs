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

        public RequestRefundService(IUnitOfWork unitOfWork, IMapper mapper, IClassService classService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _classService = classService;
        }

        // Lấy danh sách các yêu cầu hoàn tiền với điều kiện tìm kiếm và phân trang
        public async Task<BasePaginatedList<ResponseRequestRefundModelViews>> GetAllRequestRefundsAsync(int pageNumber, int pageSize, string? requestId, Guid? accountId, string? status, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<RequestRefund> requestRefundsQuery = _unitOfWork.GetRepository<RequestRefund>().Entities
                .Where(p => !p.DeletedTime.HasValue || string.IsNullOrEmpty(p.DeletedBy))
                .OrderByDescending(r => r.CreatedTime);

            // Điều kiện tìm kiếm theo requestId nếu có
            if (!string.IsNullOrWhiteSpace(requestId))
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Id == requestId);
            }

            // Điều kiện tìm kiếm theo accountId nếu có
            if (accountId.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.AccountId == accountId);
            }

            // Điều kiện tìm kiếm theo trạng thái (status) nếu có
            if (!string.IsNullOrWhiteSpace(status))
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.Status == status);
            }

            // Điều kiện lọc theo ngày bắt đầu nếu có
            if (startDate.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.CreatedTime >= startDate.Value);
            }

            // Điều kiện lọc theo ngày kết thúc nếu có
            if (endDate.HasValue)
            {
                requestRefundsQuery = requestRefundsQuery.Where(r => r.CreatedTime <= endDate.Value);
            }

            // Đếm tổng số bản ghi phù hợp với điều kiện tìm kiếm
            int totalCount = await requestRefundsQuery.CountAsync();

            // Áp dụng phân trang nếu không tìm kiếm theo requestId
            if (string.IsNullOrWhiteSpace(requestId))
            {
                requestRefundsQuery = requestRefundsQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            List<RequestRefund> paginatedRequestRefunds = await requestRefundsQuery.ToListAsync();

            var response = _mapper.Map<List<ResponseRequestRefundModelViews>>(paginatedRequestRefunds);

            return new BasePaginatedList<ResponseRequestRefundModelViews>(response, totalCount, pageNumber, pageSize);
        }

        // Tạo một yêu cầu hoàn tiền mới với các tham số chuyền vào
        public async Task<ResponseRequestRefundModelViews> CreateRequestRefundAsync(CreateRequestRefundModelViews model)
        {
            // Kiểm tra AccountId không được để trống hoặc giá trị hợp lệ
            if (model.AccountId == Guid.Empty)
            {
                throw new Exception("Vui lòng nhập AccountId hợp lệ.");
            }

            // Kiểm tra ClassId không được để trống
            if (string.IsNullOrWhiteSpace(model.ClassId))
            {
                throw new Exception("Vui lòng nhập ClassId hợp lệ.");
            }

            // Kiểm tra Status không được để trống
            if (string.IsNullOrWhiteSpace(model.Status))
            {
                throw new Exception("Vui lòng nhập trạng thái (Status) hợp lệ.");
            }

            // Kiểm tra sự tồn tại của Account
            bool isExistAccount = await _unitOfWork.GetRepository<Accounts>().Entities
                .AnyAsync(a => a.Id == model.AccountId && !a.DeletedTime.HasValue);

            if (!isExistAccount)
            {
                throw new Exception("Không tìm thấy Account hoặc tài khoản đã bị xóa!");
            }

            // Kiểm tra sự tồn tại của Class
            bool isExistClass = await _unitOfWork.GetRepository<Class>().Entities
                .AnyAsync(c => c.Id == model.ClassId && !c.DeletedTime.HasValue);

            if (!isExistClass)
            {
                throw new Exception("Không tìm thấy lớp học (Class) hoặc lớp học đã bị xóa!");
            }

            // Sử dụng AutoMapper để ánh xạ từ model sang thực thể RequestRefund
            RequestRefund newRequestRefund = _mapper.Map<RequestRefund>(model);

            // Thiết lập các thuộc tính bổ sung
            newRequestRefund.Id = Guid.NewGuid().ToString("N");
            //Accounts accounts = await _unitOfWork.GetRepository<Accounts>().GetByIdAsync(_accountUtil.GetCurrentUser);
            //newRequestRefund.CreatedBy = accounts.Id.ToString("N");
            newRequestRefund.CreatedBy = "clam Admin";
            newRequestRefund.Amount = await _classService.CalculateTotalAmount(model.ClassId);
            newRequestRefund.CreatedTime = DateTimeOffset.UtcNow;
            newRequestRefund.LastUpdatedTime = DateTimeOffset.UtcNow;

            // Thêm bản ghi RequestRefund vào cơ sở dữ liệu
            await _unitOfWork.GetRepository<RequestRefund>().InsertAsync(newRequestRefund);
            await _unitOfWork.SaveAsync();

            // Trả về kết quả sau khi tạo
            return _mapper.Map<ResponseRequestRefundModelViews>(newRequestRefund);
        }
    }
}
