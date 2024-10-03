
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Repositories.Context;

using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.Booking;

namespace OnDemandTutor.Services.Service
{
    public class BookingService : IBookingService
    {
        private readonly DatabaseContext _context;

        public BookingService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> BookSubject(BookingDto dto)
        {
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
                return "Student not found";

            var subject = await _context.Subjects.FindAsync(dto.SubjectId);
            if (subject == null)
                return "Subject not found";

            var tutorSubject = await _context.TutorSubjects.FindAsync(dto.TutorSubjectId);
            if (tutorSubject == null)
                return "Tutor for this subject not found";

            var booking = new Booking
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                TutorSubjectId = dto.TutorSubjectId,
                BookingDate = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return "Subject booked successfully";
        }
    }
}
