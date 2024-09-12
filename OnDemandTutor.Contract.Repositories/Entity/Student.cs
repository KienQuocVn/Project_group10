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
    public class Student : BaseEntity
    {
        //public int Id { get; set; }
        public int AccountId { get; set; }
        public int Yob { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public int Grade { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        //public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
