using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.ModelViews.Booking;

namespace OnDemandTutor.Contract.Services.Interface
{
  
        public interface IBookingService
        {
            Task<string> BookSubject(BookingDto dto);
        }

}
