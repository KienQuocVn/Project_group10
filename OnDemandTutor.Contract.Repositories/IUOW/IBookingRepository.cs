using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;

namespace OnDemandTutor.Contract.Repositories.IUOW
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
    }
}
