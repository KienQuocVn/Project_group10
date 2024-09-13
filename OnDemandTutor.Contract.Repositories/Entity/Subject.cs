using OnDemandTutor.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Subject : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[Required]
        //public string Name { get; set; }

        //// Navigation properties
        //public virtual ICollection<TutorSubject> TutorSubjects { get; set; }
        //public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<TutorSubject> TutorSubjects { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
