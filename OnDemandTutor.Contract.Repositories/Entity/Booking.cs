

using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Booking : BaseEntity
    {
<<<<<<< HEAD
        public Guid StudentId { get; set; }
        public Guid TutorId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid SlotId { get; set; }
=======
        //public string Id { get; set; } = Guid.NewGuid().ToString(); // thay đổi
        public Guid StudentId { get; set; }
        public virtual Accounts Student { get; set; }

        public string SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

>>>>>>> 991e0be3e63b3e2c7914793e6fa4efd988597bf0
        public DateTime BookingDate { get; set; }
        public double TotalPrice { get; set; }

        // Navigation properties
        public virtual Accounts Student { get; set; }
        public virtual TutorSubject TutorSubject { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
