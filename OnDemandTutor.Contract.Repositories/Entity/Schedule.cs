using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Schedule : BaseEntity
    {
        public Guid StudentId { get; set; }
        public String SlotId { get; set; }
        public string Status { get; set; } 

        // Navigation properties
        public virtual Accounts Student { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
