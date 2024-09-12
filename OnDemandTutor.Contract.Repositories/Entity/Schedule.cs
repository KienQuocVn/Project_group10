﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Core.Base;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Schedule : BaseEntity
    {
        //[Key]
        //public int Id { get; set; }

        //[ForeignKey("Student")]
        //public int StudentId { get; set; }

        //[ForeignKey("Slot")]
        //public int SlotId { get; set; }

        //public string Status { get; set; }

        //// Navigation properties
        //public virtual Student Student { get; set; }
        //public virtual Slot Slot { get; set; }


        //public int StudentId { get; set; }
        //public int SlotId { get; set; }
        public string Status { get; set; } 

        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
