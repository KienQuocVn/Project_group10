using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.ComplaintModelViews
{
    public class UpdateComplaintModel
    {
        public Guid StudentId { get; set; }
        public Guid TutorId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
    }
}

