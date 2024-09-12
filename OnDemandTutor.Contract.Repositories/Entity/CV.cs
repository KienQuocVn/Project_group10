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
    public class CV : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[ForeignKey("Tutor")]
        //public int TutorId { get; set; }

        //[ForeignKey("Moderator")]
        //public int ModId { get; set; }

        //public string PhoneNumber { get; set; }
        //public string Location { get; set; }
        //public string Gender { get; set; }
        //public string Experience { get; set; }
        //public string Grade { get; set; }
        //public DateTime CreateTime { get; set; }
        //public string Content { get; set; }
        //public string Url { get; set; }

        //// Navigation properties
        //public virtual Tutor Tutor { get; set; }
        //public virtual Moderator Moderator { get; set; }
        //public int Id { get; set; }
        //public int TutorId { get; set; }
        public int ModId { get; set; }
        public string PhoneNumber { get; set; }
        public int Yob { get; set; }
        public string Location { get; set; }
        public string PersonalId { get; set; }
        public string Gender { get; set; }
        public int Experience { get; set; }
        public string Grade { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public string Url { get; set; }

        // Navigation properties
        public virtual Tutor Tutor { get; set; }
        public virtual Moderator Moderator { get; set; }
    }
}
