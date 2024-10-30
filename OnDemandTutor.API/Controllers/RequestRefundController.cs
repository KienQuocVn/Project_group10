using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.RequestRefundModelViews;
using System;
using System.Threading.Tasks;
using OnDemandTutor.Core.Base;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestRefundController : ControllerBase
    {
        private readonly IRequestRefundService _requestRefundService;

        public RequestRefundController(IRequestRefundService requestRefundService)
        {
            _requestRefundService = requestRefundService;
        }

        // GET: api/RequestRefund
        // API này dành cho Admin để lấy danh sách các yêu cầu hoàn tiền với khả năng phân trang và lọc dữ liệu.
        // Các tham số lọc bao gồm: ID yêu cầu (requestId), ID tài khoản (accountId), trạng thái (status), 
        // thời gian bắt đầu và kết thúc (startDate, endDate), số tiền tối thiểu (minAmount) và số tiền tối đa (maxAmount).
        [HttpGet("admin")]
        public async Task<ActionResult<BasePaginatedList<ResponseRequestRefundModelViews>>> GetAllRequestRefundsForAdmin(
            int pageNumber = 1, int pageSize = 5, string? requestId = null, Guid? accountId = null,
            string? status = null, DateTime? startDate = null, DateTime? endDate = null,
            double? minAmount = null, double? maxAmount = null)
        {
            try
            {
                // Gọi service để lấy danh sách yêu cầu hoàn tiền cho Admin
                var result = await _requestRefundService.GetAllRequestRefundsForAdminAsync(
                    pageNumber, pageSize, requestId, accountId, status, startDate, endDate, minAmount, maxAmount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/RequestRefund/user
        // API này dành cho User để lấy danh sách các yêu cầu hoàn tiền với khả năng phân trang và lọc dữ liệu.
        // Các tham số lọc bao gồm: trạng thái (status), thời gian bắt đầu và kết thúc (startDate, endDate), 
        // số tiền tối thiểu (minAmount) và số tiền tối đa (maxAmount).
        [HttpGet("user")]
        public async Task<ActionResult<BasePaginatedList<ResponseRequestRefundModelViews>>> GetAllRequestRefundsForUser(
            int pageNumber = 1, int pageSize = 5, string? status = null,
            DateTime? startDate = null, DateTime? endDate = null,
            double? minAmount = null, double? maxAmount = null)
        {
            try
            {
                // Gọi service để lấy danh sách yêu cầu hoàn tiền cho User
                var result = await _requestRefundService.GetAllRequestRefundsForUsernAsync(
                    pageNumber, pageSize, status, startDate, endDate, minAmount, maxAmount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/RequestRefund
        // API này tạo mới một yêu cầu hoàn tiền dựa trên thông tin từ client gửi lên.
        [HttpPost]
        public async Task<ActionResult<ResponseRequestRefundModelViews>> CreateRequestRefund([FromBody] CreateRequestRefundModelViews model)
        {
            try
            {
                // Gọi service để tạo yêu cầu hoàn tiền mới
                var result = await _requestRefundService.CreateRequestRefundAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/{RequestRefundId}
        // Cập nhật trạng thái hoặc mô tả cho yêu cầu hoàn tiền
        [HttpPut("{requestId}")]
        public async Task<ActionResult<ResponseRequestRefundModelViews>> UpdateRequestRefund(string requestId, [FromBody] UpdateRequestRefundModelViews model)
        {
            try
            {
                var result = await _requestRefundService.UpdateRequestRefundAsync(requestId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/RequestRefund/{requestId}
        // thực hiện xóa mềm 1 request refund
        [HttpDelete("{requestId}")]
        public async Task<IActionResult> DeleteRequestRefund(string requestId)
        {
            try
            {
                await _requestRefundService.DeleteRequestRefundAsync(requestId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
