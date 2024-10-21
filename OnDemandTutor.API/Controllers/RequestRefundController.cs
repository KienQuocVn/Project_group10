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
        // Lấy danh sách các yêu cầu hoàn tiền có điều kiện lọc hoặc không
        [HttpGet()]
        public async Task<ActionResult<BasePaginatedList<ResponseRequestRefundModelViews>>> GetAllRequestRefunds(int pageNumber = 1, int pageSize = 5, string? requestId = null, Guid? accountId = null, string? status = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Gọi service với các tham số tìm kiếm
                var result = await _requestRefundService.GetAllRequestRefundsAsync(pageNumber, pageSize, requestId, accountId, status, startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/RequestRefund
        // Tạo mới một yêu cầu hoàn tiền
        [HttpPost()]
        public async Task<ActionResult<ResponseRequestRefundModelViews>> CreateRequestRefund([FromBody] CreateRequestRefundModelViews model)
        {
            try
            {
                ResponseRequestRefundModelViews result = await _requestRefundService.CreateRequestRefundAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
