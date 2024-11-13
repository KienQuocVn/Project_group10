using OnDemandTutor.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Subject : BaseEntity
    {
        public Guid TutorId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<TutorSubject> TutorSubjects { get; set; }

        [JsonIgnore]
        public virtual ICollection<Class> Classes { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
