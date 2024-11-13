﻿using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class TutorSubject : BaseEntity
    {
        [Key]
        public Guid TutorId { get; set; }
        public Guid UserId { get; set; }
        public string SubjectId { get; set; }
        public string Bio { get; set; }
        public double Rating { get; set; }
        public int Experience { get; set; }
        public decimal HourlyRate { get; set; }

        [JsonIgnore]
        public virtual Accounts User { get; set; }

        [JsonIgnore]
        public virtual Subject Subject { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
