using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using OnDemandTutor.API.VNPayLibrary;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;
using System;
using System.Threading.Tasks;
using PaymentResponse = OnDemandTutor.ModelViews.UserModelViews.PaymentResponse;


namespace OnDemandTutor.Services.Service
{
    public class VnPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VnPayService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreatePaymentUrl(PaymentInfo model)
        {
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor.HttpContext));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", model.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", model.OrderType);
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", model.TxnRef);

            string paymentUrl = vnpay.CreateRequestUrl(_configuration["VnPay:PaymentUrl"], _configuration["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public string CreatePaymentUrl(PaymentInfo model, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public PaymentResponse ProcessPaymentCallback(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = collections["vnp_SecureHash"].ToString();
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VnPay:HashSecret"]);

            if (checkSignature)
            {
                return new PaymentResponse
                {
                    Success = vnpay.GetResponseData("vnp_ResponseCode") == "00",
                    PaymentMethod = "VnPay",
                    OrderDescription = vnpay.GetResponseData("vnp_OrderInfo"),
                    OrderId = vnpay.GetResponseData("vnp_TxnRef"),
                    PaymentId = vnpay.GetResponseData("vnp_TransactionNo"),
                    TransactionId = vnpay.GetResponseData("vnp_TransactionNo"),
                    Token = vnpay.GetResponseData("vnp_SecureHash"),
                    Amount = Convert.ToDouble(vnpay.GetResponseData("vnp_Amount")) / 100
                };
            }

            return new PaymentResponse { Success = false };
        }

    }
}