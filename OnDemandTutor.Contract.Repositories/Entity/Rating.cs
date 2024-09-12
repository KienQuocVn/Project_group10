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
    public class Rating : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[ForeignKey("Student")]
        //public int StudentId { get; set; }

        //[ForeignKey("Tutor")]
        //public int TutorId { get; set; }

        //public int? RatingValue { get; set; }

        //// Navigation properties
        //public virtual Student Student { get; set; }
        //public virtual Tutor Tutor { get; set; }
        //public int Id { get; set; }


        //public int TutorId { get; set; }
        //public int StudentId { get; set; }
        //public int SubjectId { get; set; }
        public int RatingValue { get; set; } // Rating từ 1 đến 10

        // Navigation properties
        public virtual Tutor Tutor { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
