using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Services.Service.AccountUltil;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;
        private readonly IConfiguration _configuration;
        private readonly AccountUtils _accountUtil;

        public PaymentController(IVNPayService vnPayService, IConfiguration configuration,AccountUtils accountUtils)
        {
            _vnPayService = vnPayService;
            _configuration = configuration;
            _accountUtil = accountUtils;
        }
        [HttpPost("submitOrder")]
        public IActionResult SubmitOrder([FromQuery] string amount)
        {
            try
            {
                var paymentInfo = new PaymentInfo
                {
                    Amount = double.Parse(amount),
                    OrderDescription = "Description of the Order", 
                    OrderType = "Type of the Order", 
                    TxnRef = Guid.NewGuid().ToString() 
                };

                var paymentUrl = _vnPayService.CreatePaymentUrl(paymentInfo, HttpContext);
                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpGet("vnpay-payment-return")]
        public IActionResult PaymentCompleted()
        {
            Accounts accounts = _accountUtil.GetCurrentUser();
            try
            {
                var response = _vnPayService.ProcessPaymentCallback(HttpContext.Request.Query);
                var orderInfo = HttpContext.Request.Query["vnp_OrderInfo"].ToString();
                var paymentTime = HttpContext.Request.Query["vnp_PayDate"].ToString();
                var transactionId = HttpContext.Request.Query["vnp_TransactionNo"].ToString();
                var totalPrice = HttpContext.Request.Query["vnp_Amount"].ToString();
                if (response.Success)
                {
                    accounts.UserInfo.Balance += double.Parse(totalPrice);
                    return Ok(new
                    {
                        Status = "Success",
                        OrderInfo = orderInfo,
                        PaymentTime = paymentTime,
                        TransactionId = transactionId,
                        TotalPrice = totalPrice
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Status = "Failure",
                        OrderInfo = orderInfo,
                        PaymentTime = paymentTime,
                        TransactionId = transactionId,
                        TotalPrice = totalPrice
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
       



    }
}
