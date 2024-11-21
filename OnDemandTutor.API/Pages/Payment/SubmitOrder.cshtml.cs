using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;

namespace OnDemandTutor.API.Pages.Payment
{
    public class SubmitOrderModel : PageModel
    {
        private readonly IVNPayService _vnPayService;

        [BindProperty]
        public string Amount { get; set; }
        public string? ErrorMessage { get; set; }

        public SubmitOrderModel(IVNPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Amount))
            {
                ErrorMessage = "Please enter an amount";
                return Page();
            }

            try
            {
                var paymentInfo = new PaymentInfo
                {
                    Amount = double.Parse(Amount),
                    OrderDescription = "Payment for OnDemandTutor Services",
                    OrderType = "Tutor Payment",
                    TxnRef = Guid.NewGuid().ToString()
                };

                var paymentUrl = _vnPayService.CreatePaymentUrl(paymentInfo, HttpContext);
                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
