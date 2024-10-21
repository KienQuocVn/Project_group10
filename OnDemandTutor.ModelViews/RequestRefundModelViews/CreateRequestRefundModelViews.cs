using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.RequestRefundModelViews
{
    public class CreateRequestRefundModelViews
    {
        public Guid AccountId { get; set; }
        public String ClassId { get; set; }
        public String Status { get; set; }
        public String Description { get; set; }

    }
}
