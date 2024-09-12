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
    public class BanAccount : BaseEntity
    {

        //[ForeignKey("Moderator")]
        public int ModId { get; set; }

        //[ForeignKey("Account")]
        public int AccountId { get; set; }

        public int AmountOfReport { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public virtual Moderator Moderator { get; set; }
        public virtual Account Account { get; set; }
    }
}
