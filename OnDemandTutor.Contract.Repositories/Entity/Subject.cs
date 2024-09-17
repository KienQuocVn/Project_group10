using OnDemandTutor.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Subject : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}