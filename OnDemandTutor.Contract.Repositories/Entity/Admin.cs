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
    public class Admin : BaseEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        // Navigation property
        public virtual Account Account { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        //public virtual ICollection<Payment> Payments { get; set; }
        //public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
