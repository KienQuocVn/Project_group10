using OnDemandTutor.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Feedback : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[ForeignKey("Moderator")]
        //public int ModId { get; set; }

        //[ForeignKey("Student")]
        //public int StudentId { get; set; }

        //[ForeignKey("Slot")]
        //public int SlotId { get; set; }

        //public string FeedbackText { get; set; }
        //public DateTime CreatedAt { get; set; }

        //// Navigation properties
        //public virtual Moderator Moderator { get; set; }
        //public virtual Student Student { get; set; }
        //public virtual Slot Slot { get; set; }
        //public int Id { get; set; }


        //public int ModId { get; set; }
        //public int StudentId { get; set; }
        //public int SlotId { get; set; }
        //public int TutorId { get; set; }
        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Moderator Moderator { get; set; }
        public virtual Student Student { get; set; }
        public virtual Slot Slot { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}
