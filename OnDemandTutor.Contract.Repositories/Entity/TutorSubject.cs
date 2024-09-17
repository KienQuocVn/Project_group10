using OnDemandTutor.Core.Base;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class TutorSubject : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TutorId { get; set; }
        public string SubjectId { get; set; }

        public virtual Accounts Tutor { get; set; }
        public virtual Subject Subject { get; set; }
    }
}