using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Schema;
using OnDemandTutor.ModelViews.AuthModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PaymentResponse = OnDemandTutor.ModelViews.UserModelViews.PaymentResponse;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(PaymentInfo model, HttpContext context);
        PaymentResponse ProcessPaymentCallback(IQueryCollection collections);
        
    }
}
