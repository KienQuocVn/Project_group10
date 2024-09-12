using OnDemandTutor.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Payment : BaseEntity
    {
        //public int Id { get; set; }
        //public int TutorId { get; set; }
        //public int AdminId { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }

        // Navigation properties
        public virtual Tutor Tutor { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
