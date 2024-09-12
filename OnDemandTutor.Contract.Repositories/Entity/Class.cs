using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Core.Base;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Class : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[ForeignKey("Tutor")]
        //public int TutorId { get; set; }

        //[ForeignKey("Subject")]
        //public int SubjectId { get; set; }

        //public int AmountOfSlot { get; set; }
        //public DateTime? StartDay { get; set; }
        //public DateTime? EndDay { get; set; }

        //// Navigation properties
        //public virtual Tutor Tutor { get; set; }
        //public virtual Subject Subject { get; set; }
        //public int Id { get; set; }
        //public int TutorId { get; set; }
        //public int SubjectId { get; set; }
        public int AmountOfSlot { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }

        // Navigation properties
        public virtual Tutor Tutor { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
