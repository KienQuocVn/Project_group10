using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;
        private readonly IConfiguration _configuration;

        public PaymentController(IVNPayService vnPayService, IConfiguration configuration)
        {
            _vnPayService = vnPayService;
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            return Ok("createOrder");
        }

        // Chuyển hướng người dùng đến cổng thanh toán VNPAY
        [HttpPost("submitOrder")]
        public IActionResult SubmitOrder([FromQuery] string amount)
        {
            try
            {
                var paymentUrl = _vnPayService.CreatePaymentUrl(new PaymentInfo { Amount = double.Parse(amount) }, HttpContext);
                return Ok(new { Url = paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Sau khi hoàn tất thanh toán, VNPAY sẽ chuyển hướng trình duyệt về URL này
        [HttpGet("vnpay-payment-return")]
        public IActionResult PaymentCompleted()
        {
            try
            {
                var response = _vnPayService.ProcessPaymentCallback(HttpContext.Request.Query);
                var orderInfo = HttpContext.Request.Query["vnp_OrderInfo"].ToString();
                var paymentTime = HttpContext.Request.Query["vnp_PayDate"].ToString();
                var transactionId = HttpContext.Request.Query["vnp_TransactionNo"].ToString();
                var totalPrice = HttpContext.Request.Query["vnp_Amount"].ToString();

                if (response.Success)
                {
                    var successUrl = _configuration["VnPay:ReturnSuccessUrl"];
                    if (string.IsNullOrEmpty(successUrl))
                    {
                        return BadRequest(new { Error = "Value cannot be null or empty. (Parameter 'url')" });
                    }
                    return Redirect(successUrl);
                }
                else
                {
                    var failureUrl = _configuration["VnPay:ReturnFailureUrl"];
                    if (string.IsNullOrEmpty(failureUrl))
                    {
                        return BadRequest(new { Error = "Value cannot be null or empty. (Parameter 'url')" });
                    }
                    return Redirect(failureUrl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

    }
}
