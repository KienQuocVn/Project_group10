using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;

namespace OnDemandTutor.Repositories.UOW
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
