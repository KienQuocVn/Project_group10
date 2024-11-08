using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;
using System.Text.Json.Serialization;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Class : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string SubjectId { get; set; }
        public int AmountOfSlot { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }

        // Bỏ qua việc tuần tự hóa mối quan hệ với Accounts để tránh vòng lặp
        [JsonIgnore]
        public virtual Accounts account { get; set; }

        // Bỏ qua việc tuần tự hóa mối quan hệ với Subject để tránh vòng lặp
        [JsonIgnore]
        public virtual Subject Subject { get; set; }

        // Bỏ qua việc tuần tự hóa mối quan hệ với Slots
        [JsonIgnore]
        public virtual ICollection<Slot> Slots { get; set; }

        // Bỏ qua việc tuần tự hóa mối quan hệ với Feedbacks
        [JsonIgnore]
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        // Bỏ qua việc tuần tự hóa mối quan hệ với Complaints
        [JsonIgnore]
        public virtual ICollection<Complaint> Complaints { get; set; }
    }
}
