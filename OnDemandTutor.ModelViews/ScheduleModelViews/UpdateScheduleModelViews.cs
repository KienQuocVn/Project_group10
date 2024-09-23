using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.ModelViews.ScheduleModelViews
{
    public class UpdateScheduleModelViews
    {

        [Required]
        public string Status { get; set; } // Trạng thái mới

        [Required]
        public string StudentId { get; set; } // ID của Student mới

        [Required]
        public string SlotId { get; set; } // ID của Slot mới
    }
}
