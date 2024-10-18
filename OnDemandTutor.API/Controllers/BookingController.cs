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
        [HttpPost("book-subject/slot")]
        public async Task<IActionResult> BookSubjectBySlot([FromBody] SlotBookingDto dto)
        {
            // Kiểm tra xem ModelState có hợp lệ không  
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Nếu không hợp lệ, trả về mã lỗi 400 kèm theo thông tin lỗi  

            // Gọi phương thức đặt chỗ theo slot từ dịch vụ  
            var result = await _bookingService.BookSubjectBySlot(dto);
            // Kiểm tra xem kết quả có chứa thông báo "not found" không  
            if (result.Contains("not found"))
                return NotFound(new { message = result }); // Nếu có, trả về mã lỗi 404 kèm theo thông điệp  

            // Nếu thành công, trả về mã 200 kèm theo thông điệp thành công  
            return Ok(new { message = result });
        }

        // POST api/booking/book-subject/time  
        [HttpPost("book-subject/time")]
        public async Task<IActionResult> BookSubjectByTime([FromBody] TimeBookingDto dto)
        {
            // Kiểm tra xem ModelState có hợp lệ không  
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Nếu không hợp lệ, trả về mã lỗi 400 kèm theo thông tin lỗi  

            // Gọi phương thức đặt chỗ theo thời gian từ dịch vụ  
            var result = await _bookingService.BookSubjectByTime(dto);
            // Kiểm tra xem kết quả có chứa thông báo "not found" không  
            if (result.Contains("not found"))
                return NotFound(result); // Nếu có, trả về mã lỗi 404 kèm theo thông điệp  

            // Nếu thành công, trả về mã 200 kèm theo thông điệp thành công  
            return Ok(result);
        }
    }
/*{
  "studentId": "78270973-0EF2-417F-A252-A1ACC525A86B",
  "subjectId": "2",
  "tutorSubjectId": "B247E964-07F1-4763-A58D-FED6FBCC1AC0",
  "slotId": "2",
  "selectedDate": "2024-10-18T04:46:08.231Z"
}*/

/*{  
  "studentId": "EBE55496-3796-43D8-A305-29F9C0FBF9DA",  
  "subjectId": "1", 
  "tutorSubjectId": "5FA01B97-8E21-4A62-8BDD-A6712FC30AB3", 
  "slotId": "1",
  "selectedDate": "2024-10-18T04:14:45.353Z",  
  "startTime": "14:00:00",  
  "endTime": "15:00:00"  
}*/
}
