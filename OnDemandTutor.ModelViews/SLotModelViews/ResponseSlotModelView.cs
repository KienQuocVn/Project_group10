using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.SLotModelViews
{
    public class ResponseSlotModelView
    {
        public string Id { get; set; }
        public string ClassId { get; set; }
        public string DayOfSlot { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double Price { get; set; }

        public string? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTimeOffset LastUpdateTime { get; set; }
    }
}
