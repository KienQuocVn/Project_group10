using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OnDemandTutor.ModelViews.TutorSubjectModelViews
{
    public class CreateTutorSubjectModelViews
    {
        public Guid TutorId { get; set; }
        public string SubjectId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
