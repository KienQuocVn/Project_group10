using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.UserModelViews;
using OnDemandTutor.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    internal class OderService : IOrderService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVNPayService _vnPayService;

        public OderService(IUnitOfWork unitOfWork, VnPayService vnPayService)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;

        }
        public async Task<bool> OnPaymentByWalletSuccess(PaymentInfomation paymentInfomation)
        {
            var userRepository = _unitOfWork.GetRepository<Accounts>();
            var user = await userRepository.GetByIdAsync(paymentInfomation.AccountId);

            if (user == null) return false;

            if (user.UserInfo != null)
            {
                if(user.UserInfo.Balance > paymentInfomation.Amount) {
                    user.UserInfo.Balance -= paymentInfomation.Amount;
                }
                else { return false; }
            }

            await userRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
