using Microsoft.AspNetCore.Identity;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Utils;
using System.Text.Json.Serialization;

namespace OnDemandTutor.Repositories.Entity
{
    public class Accounts : IdentityUser<Guid>
    {
        public string Password {  get; set; } = string.Empty;
        public virtual UserInfo? UserInfo { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<TutorSubject> TutorSubjects { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        [JsonIgnore] 
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Class> Classes { get; set; }

        public Accounts()
        {
            CreatedTime = CoreHelper.SystemTimeNow;
            LastUpdatedTime = CreatedTime;
        }
    }
}
