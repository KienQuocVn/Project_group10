using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.Booking;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST api/booking/book-subject
        [HttpPost("book-subject")]
        public async Task<IActionResult> BookSubject([FromBody] BookingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookingService.BookSubject(dto);
            if (result == "Student not found" || result == "Subject not found" || result == "Tutor for this subject not found")
                return NotFound(result);

            return Ok(result);
        }
    }
}
