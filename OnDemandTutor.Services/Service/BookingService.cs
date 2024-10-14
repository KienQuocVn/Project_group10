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

        public async Task<string> BookSubject(BookingDto dto)
        {
            var student = await _unitOfWork.GetRepository<Accounts>().FindAsync(dto.StudentId);
            if (student == null)
                return "Student not found";

            var subject = await _unitOfWork.SubjectRepository.FindAsync(dto.SubjectId);
            if (subject == null)
                return "Subject not found";

            TutorSubject tutorSubject = null;

            // Nếu người dùng chọn gia sư  
            if (dto.TutorSubjectId.HasValue)
            {
                tutorSubject = await _unitOfWork.TutorRepository.Entities
                    .FirstOrDefaultAsync(ts => ts.TutorId == dto.TutorSubjectId.Value);

                if (tutorSubject == null)
                    return "Tutor for this subject not found";
            }
            else
            {
                // Nếu người dùng không chọn gia sư, có thể tự sắp xếp  
                // Lấy danh sách gia sư cho môn học  
                var availableTutors = await _unitOfWork.TutorRepository.Entities
                    .Where(t => t.SubjectId == dto.SubjectId) // Giả sử có thuộc tính SubjectId trong Tutor  
                    .ToListAsync();

                if (!availableTutors.Any())
                    return "No available tutors for this subject.";

                // Chọn gia sư đầu tiên trong danh sách có sẵn (hoặc có thể thêm logic chọn ngẫu nhiên hoặc theo tiêu chí)  
                tutorSubject = availableTutors.First();
            }

            var slot = await _unitOfWork.SlotRepository.Entities
                .FirstOrDefaultAsync(s => s.Id == dto.SlotId.ToString());
            if (slot == null)
                return "Slot not found";

            // Kiểm tra xem ngày đã chọn có rảnh không  
            if (!await IsDateAvailable(tutorSubject.TutorId, dto.SelectedDate, slot.StartTime, slot.EndTime))
            {
                return "The selected date is not available for this tutor.";
            }

            double durationInHours = (slot.EndTime - slot.StartTime).TotalHours;
            double totalPrice = slot.Price * durationInHours;

            // Tạo một lớp mới  
            var newClass = new Class
            {
                AccountId = tutorSubject.TutorId,
                SubjectId = dto.SubjectId.ToString(),
                AmountOfSlot = 20,
                StartDay = dto.SelectedDate, // Sử dụng ngày được chọn  
                EndDay = dto.SelectedDate.AddMonths(1), // Giả sử lớp học kéo dài 1 tháng  
            };

            // Thêm lớp vào cơ sở dữ liệu  
            await _unitOfWork.ClassRepository.InsertAsync(newClass);
            await _unitOfWork.SaveAsync();

            // Tạo slot cho các ngày đã chọn  
            var newSlot = new Slot
            {
                ClassId = newClass.Id,
                StartTime = slot.StartTime, // Giữ nguyên thời gian bắt đầu  
                EndTime = slot.EndTime, // Giữ nguyên thời gian kết thúc  
                Price = slot.Price,
                DayOfSlot = dto.SelectedDate.DayOfWeek.ToString() // Ngày của slot  
            };

            // Thêm slot vào cơ sở dữ liệu  
            await _unitOfWork.SlotRepository.InsertAsync(newSlot);
            await _unitOfWork.SaveAsync();

            // Đặt booking cho môn học này  
            var booking = new Booking
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                TutorId = tutorSubject.TutorId,
                SlotId = dto.SlotId,
                BookingDate = DateTime.UtcNow,
                TotalPrice = totalPrice
            };

            await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
            await _unitOfWork.SaveAsync();

            return $"Subject booked successfully and class has been scheduled. Total Price: {totalPrice:C}";
        }

        // Phương thức kiểm tra khả dụng của ngày  
        private async Task<bool> IsDateAvailable(Guid tutorId, DateTime selectedDate, TimeSpan startTime, TimeSpan endTime)
        {
            var existingSlots = await _unitOfWork.SlotRepository.Entities
                .Where(s => s.DayOfSlot == selectedDate.DayOfWeek.ToString())
                .ToListAsync();

            foreach (var slot in existingSlots)
            {
                if ((startTime < slot.EndTime) && (endTime > slot.StartTime))
                {
                    return false; // Ngày không rảnh  
                }
            }

            return true; // Ngày rảnh  
        }

    }
}
