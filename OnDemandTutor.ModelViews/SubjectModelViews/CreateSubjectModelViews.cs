using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.SubjectModelViews
{
    public class CreateSubjectModelViews
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
