using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class RequestRefund : BaseEntity
    {
        public Guid AccountId { get; set; }
        public String Status { get; set; }
        public double Amount { get; set; }

        public String? Description { get; set; } 

        public virtual Accounts Accounts { get; set; }


    }
}
