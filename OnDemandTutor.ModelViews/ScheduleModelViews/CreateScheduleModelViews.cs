using System;
using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.ModelViews.ScheduleModelViews
{
    public class CreateScheduleModelViews
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string SlotId { get; set; }


    }
}
