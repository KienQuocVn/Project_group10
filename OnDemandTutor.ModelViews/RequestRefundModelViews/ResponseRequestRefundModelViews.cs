using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.RequestRefundModelViews
{
    public class ResponseRequestRefundModelViews
    {
        public string Id { get; set; }
        public Guid AccountId { get; set; }
        public String ClassId { get; set; }
        public String Status { get; set; }
        public double Amount { get; set; }

        public String Description { get; set; }

        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
    }
}
