
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.Booking
{
    
        public class BookingDto
        {

            public Guid StudentId { get; set; }
            public String SubjectId { get; set; }
            public String TutorSubjectId { get; set; }
             public String SlotId { get; set; }
    }

    

}
