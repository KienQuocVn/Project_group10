
﻿

using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Booking : BaseEntity
    {

        public Guid StudentId { get; set; }
        public Guid TutorId { get; set; }
        public String SubjectId { get; set; }
        public String SlotId { get; set; }
        public DateTime BookingDate { get; set; }
        public double TotalPrice { get; set; }

        // Navigation properties
        public virtual Accounts Student { get; set; }
        public virtual TutorSubject TutorSubject { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
