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
    public class Moderator : BaseEntity
    {
        //public int Id { get; set; }
        public int AccountId { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<CV> CVs { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<BanAccount> BanAccounts { get; set; }
        //public virtual ICollection<ReasonDenyCv> ReasonDenyCvs { get; set; }
    }
}
