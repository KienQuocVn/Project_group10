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
        public Guid SubjectId { get; set; }
    }
}
