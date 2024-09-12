using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Tutor : BaseEntity
    {
        //public int Id { get; set; }
        public int AccountId { get; set; }
        public string Active { get; set; } // đã kiểm duyệt, chưa kiểm duyệt, bị từ chối

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<CV> CVs { get; set; }
        public virtual ICollection<TutorSubject> TutorSubjects { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        //public virtual ICollection<Payment> Payments { get; set; }
    }
}
