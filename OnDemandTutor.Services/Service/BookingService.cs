using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.Booking;
using OnDemandTutor.Repositories.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;


namespace OnDemandTutor.Services.Service

{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> BookSubjectBySlot(SlotBookingDto dto)
        {
            // Kiểm tra xem dữ liệu đặt chỗ có null không và trả về thông báo lỗi nếu có  
            if (dto == null)
                return "Booking data is null"; // Trả về thông báo lỗi nếu dữ liệu đặt chỗ không hợp lệ  

            // Lấy thông tin sinh viên dựa trên StudentId từ DTO  
            var student = await _unitOfWork.GetRepository<Accounts>().FindAsync(dto.StudentId);
            // Kiểm tra xem sinh viên có tồn tại không  
            if (student == null)
                return "Student not found"; // Trả về thông báo lỗi nếu không tìm thấy sinh viên  

            // Lấy thông tin môn học dựa trên SubjectId từ DTO  
            var subject = await _unitOfWork.SubjectRepository.FindAsync(dto.SubjectId);
            // Kiểm tra xem môn học có tồn tại không  
            if (subject == null)
                return "Subject not found"; // Trả về thông báo lỗi nếu không tìm thấy môn học  

            // Tìm thông tin TutorSubject bao gồm cả UserId  
            var tutorSubject = await _unitOfWork.TutorRepository.Entities
                .FirstOrDefaultAsync(ts => ts.TutorId == dto.TutorSubjectId);

            // Kiểm tra xem giáo viên cho môn học có tồn tại không  
            if (tutorSubject == null)
                return "Tutor for this subject not found"; // Trả về thông báo lỗi nếu không tìm thấy giáo viên cho môn học  

            // Lấy thông tin slot đã chọn dựa trên SlotId từ DTO  
            var selectedSlot = await _unitOfWork.SlotRepository.FindAsync(dto.SlotId);
            // Kiểm tra xem slot đã chọn có tồn tại không  
            if (selectedSlot == null)
                return "Selected slot not found"; // Trả về thông báo lỗi nếu không tìm thấy slot đã chọn  

            // Kiểm tra xem có đặt chỗ nào đã tồn tại trong slot đã chọn cho ngày đã chọn không  
            var existingBookings = await _unitOfWork.GetRepository<Booking>().Entities
                .Where(b => b.SlotId == selectedSlot.Id.ToString() && b.BookingDate.Date == dto.SelectedDate.Date)
                .ToListAsync();

            // Nếu có đặt chỗ đã tồn tại cho slot, trả về thông báo lỗi  
            if (existingBookings.Any())
            {
                return "The selected slot is already booked.";
            }

            // Tạo một đối tượng đặt chỗ mới với các thông tin liên quan  
            var booking = new Booking
            {
                StudentId = dto.StudentId, // Gán StudentId từ DTO  
                SubjectId = dto.SubjectId, // Gán SubjectId từ DTO  
                TutorId = tutorSubject.TutorId, // Gán TutorId từ TutorSubject  
                UserId = tutorSubject.UserId, // Gán UserId từ TutorSubject  
                SlotId = selectedSlot.Id.ToString(), // Gán SlotId từ slot đã chọn  
                BookingDate = DateTime.UtcNow, // Đặt ngày đặt chỗ là thời gian UTC hiện tại  
                TotalPrice = selectedSlot.Price, // Đặt tổng giá là giá của slot  
                StartTime = selectedSlot.StartTime, // Đặt thời gian bắt đầu của đặt chỗ  
                EndTime = selectedSlot.EndTime // Đặt thời gian kết thúc của đặt chỗ  
            };

            // Chèn đặt chỗ mới vào cơ sở dữ liệu  
            await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
            // Lưu các thay đổi vào cơ sở dữ liệu  
            await _unitOfWork.SaveAsync();

            // Trả về thông báo thành công với tổng giá được định dạng dưới dạng tiền tệ  
            return $"Subject booked successfully. Total Price: {selectedSlot.Price:C}";
        }

        public async Task<string> BookSubjectByTime(TimeBookingDto dto)
        {
            // Kiểm tra xem sinh viên có tồn tại không  
            var student = await _unitOfWork.GetRepository<Accounts>().FindAsync(dto.StudentId);
            if (student == null)
                return "Student not found";

            // Kiểm tra xem môn học có tồn tại không  
            var subject = await _unitOfWork.SubjectRepository.FindAsync(dto.SubjectId);
            if (subject == null)
                return "Subject not found";

            // Kiểm tra xem giáo viên cho môn học có tồn tại không  
            var tutorSubject = await _unitOfWork.TutorRepository.Entities
                .FirstOrDefaultAsync(ts => ts.TutorId == dto.TutorSubjectId);
            if (tutorSubject == null)
                return "Tutor for this subject not found";

            // Kiểm tra tính hợp lệ của thời gian  
            if (dto.StartTime >= dto.EndTime)
                return "Start time must be earlier than end time.";

            // Kiểm tra xem thời gian đã được đặt chưa  
            if (!await IsTimeAvailable(tutorSubject.TutorId, dto.SelectedDate, dto.StartTime, dto.EndTime))
            {
                return "The selected time is not available for this tutor.";
            }

            // Kiểm tra SlotId hợp lệ  
            var slot = await _unitOfWork.GetRepository<Slot>().FindAsync(dto.SlotId);
            if (slot == null)
            {
                return "SlotId is not valid.";
            }

            // Tính toán tổng thời gian và giá  
            double durationInHours = (dto.EndTime - dto.StartTime).TotalHours;
            double totalPrice = CalculateTotalPrice(durationInHours);

            // Tạo một booking mới  
            var booking = new Booking
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                TutorId = tutorSubject.TutorId, // Gán TutorId  
                UserId = tutorSubject.UserId, // Gán UserId từ tutorSubject  
                BookingDate = DateTime.UtcNow, // Ngày đặt chỗ hiện tại  
                TotalPrice = totalPrice, // Tổng giá đã tính  
                StartTime = dto.StartTime, // Thời gian bắt đầu  
                EndTime = dto.EndTime, // Thời gian kết thúc  
                SlotId = dto.SlotId // Cập nhật SlotId hợp lệ  
            };

            // Thêm booking vào cơ sở dữ liệu  
            await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
            // Lưu các thay đổi vào cơ sở dữ liệu  
            await _unitOfWork.SaveAsync();

            // Trả về thông báo thành công với tổng giá  
            return $"Subject booked successfully. Total Price: {totalPrice:C}";
        }

        private async Task<bool> IsTimeAvailable(Guid tutorId, DateTime selectedDate, TimeSpan startTime, TimeSpan endTime)
        {
            // Lấy danh sách các booking hiện có cho giáo viên trong ngày đã chọn  
            var existingBookings = await _unitOfWork.GetRepository<Booking>().Entities
                .Where(b => b.TutorId == tutorId && b.BookingDate.Date == selectedDate.Date)
                .ToListAsync();

            // Kiểm tra xem thời gian đã chọn có xung đột với bất kỳ booking nào không  
            foreach (var booking in existingBookings)
            {
                // Nếu thời gian đã chọn xung đột với thời gian của booking hiện có  
                if ((startTime < booking.EndTime) && (endTime > booking.StartTime))
                {
                    return false; // Thời gian không khả dụng  
                }
            }
            return true; // Thời gian khả dụng  
        }

        // Tính toán tổng giá dựa trên thời gian đã đặt  
        private double CalculateTotalPrice(double durationInHours)
        {
            // Giả sử giá giờ là một hằng số hoặc có thể lấy từ cơ sở dữ liệu  
            const decimal hourlyRate = 50m; // Ví dụ: 50 đồng/giờ  
            return durationInHours * (double)hourlyRate; // Tính tổng giá  
        }

    }
}