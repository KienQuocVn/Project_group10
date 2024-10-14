using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
namespace OnDemandTutor.ModelViews.TutorSubjectModelViews
{
    public class CreateTutorSubjectModelViews
    {
        public Guid UserId { get; set; }
        public Guid SubjectId { get; set; }
        public string Bio { get; set; }
        public double Rating { get; set; }
        public int Experience { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
