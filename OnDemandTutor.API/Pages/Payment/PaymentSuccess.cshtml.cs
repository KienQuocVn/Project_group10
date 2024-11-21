using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Services.Service.AccountUltil;
using OnDemandTutor.Repositories.UOW;

namespace OnDemandTutor.API.Pages.Payment
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly IVNPayService _vnPayService;
        private readonly AccountUtils _accountUtil;
        private readonly AuthenticationRepository _accountRepository;

        public string OrderInfo { get; set; }
        public string PaymentTime { get; set; }
        public string TransactionId { get; set; }
        public string TotalPrice { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public PaymentSuccessModel(
            IVNPayService vnPayService,
            AccountUtils accountUtils,
            AuthenticationRepository authenticationRepository)
        {
            _vnPayService = vnPayService;
            _accountUtil = accountUtils;
            _accountRepository = authenticationRepository;
        }

        public IActionResult OnGet()
        {
            try
            {
                var response = _vnPayService.ProcessPaymentCallback(HttpContext.Request.Query);

                OrderInfo = HttpContext.Request.Query["vnp_OrderInfo"].ToString();
                PaymentTime = HttpContext.Request.Query["vnp_PayDate"].ToString();
                TransactionId = HttpContext.Request.Query["vnp_TransactionNo"].ToString();
                TotalPrice = HttpContext.Request.Query["vnp_Amount"].ToString();

                if (response.Success)
                {
                    var currentUser = _accountUtil.GetCurrentUser();
                    currentUser.UserInfo.Balance += double.Parse(TotalPrice);
                    _accountRepository.Update(currentUser);
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = "Payment processing failed";
                }

                return Page();
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
