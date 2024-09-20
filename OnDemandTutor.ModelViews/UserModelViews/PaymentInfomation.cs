using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.UserModelViews
{
    public class PaymentInfomation

    {
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
        public string ClassId { get; set; }

    }
}
