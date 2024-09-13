using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class TutorSubject : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[ForeignKey("Tutor")]
        //public int TutorId { get; set; }

        //[ForeignKey("Subject")]
        //public int SubjectId { get; set; }

        //// Navigation properties
        //public virtual Tutor Tutor { get; set; }
        //public virtual Subject Subject { get; set; }
        public Guid TutorId { get; set; }
        public string SubjectId { get; set; }

        // Navigation properties
        public virtual Accounts Tutor { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
