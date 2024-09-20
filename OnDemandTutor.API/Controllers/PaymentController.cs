using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;

        public PaymentController(IVNPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] PaymentInfo model)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { Url = paymentUrl });
        }

        [HttpGet("payment-callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.ProcessPaymentCallback(HttpContext.Request.Query);
            if (response.Success)
            {
                
                return Ok(response);
            }
            else
            {
                
                return BadRequest(response);
            }
        }
    }
}
