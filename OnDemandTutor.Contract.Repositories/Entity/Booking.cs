using OnDemandTutor.Core.Base;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Booking : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid TutorId { get; set; } 
        public Guid UserId { get; set; }  
        public string SubjectId { get; set; }
        public string SlotId { get; set; }
        public DateTime BookingDate { get; set; }
        public double TotalPrice { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan StartTime { get; set; }

        public virtual Accounts Student { get; set; }
        public virtual TutorSubject TutorSubject { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
