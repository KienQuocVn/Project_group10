using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.ScheduleModelViews
{
    public class ResponseScheduleModelViews
    {
        public string Id { get; set; }
        public Guid StudentId { get; set; }
        public string SlotId { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }

    }
}
