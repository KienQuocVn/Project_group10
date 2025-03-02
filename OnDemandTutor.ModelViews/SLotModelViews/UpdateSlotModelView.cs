﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.SLotModelViews
{
    public class UpdateSlotModelView
    {
        public string SlotId { get; set; }
        public string ClassId { get; set; }
        public string DayOfSlot { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double Price { get; set; }
    }
}
