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

            var tutorSubject = await _unitOfWork.TutorRepository.Entities
              .FirstOrDefaultAsync(ts => ts.Id == dto.TutorSubjectId && ts.SubjectId == dto.SubjectId);

            if (tutorSubject == null)
                return "Tutor for this subject not found";

            var slot = await _unitOfWork.SlotRepository.Entities
              .FirstOrDefaultAsync(s => s.Id == dto.SlotId.ToString());
            if (slot == null)
                return "Slot not found";

            double durationInHours = (slot.EndTime - slot.StartTime).TotalHours;
            double totalPrice = slot.Price * durationInHours;

            // Tạo một lớp mới
            var newClass = new Class
            {
                AccountId = tutorSubject.TutorId, // Gán gia sư cho lớp
                SubjectId = dto.SubjectId.ToString(),
                AmountOfSlot = 20, // Đặt số lượng slot mặc định
                StartDay = DateTime.UtcNow, // Ngày bắt đầu lớp học
                EndDay = DateTime.UtcNow.AddMonths(1), // Giả sử lớp học kéo dài 1 tháng
            };

            // Thêm lớp vào cơ sở dữ liệu
            await _unitOfWork.ClassRepository.InsertAsync(newClass);
            await _unitOfWork.SaveAsync();

            //fix chọn ngày giờ linh động không set mặc định ngày

            // chọn ngày học rảnh 

            // linh động chọn giờ chọn lsot , xem có lớp có hay k ,,

            // linh hoạt chọn ngày vs môn học 
            for (int i = 0; i < 20; i++)
            {
                var startDateTime = DateTime.Now.Date.AddDays(i).Add(slot.StartTime);  // Bắt đầu tại thời gian trong ngày
                var endDateTime = DateTime.Now.Date.AddDays(i).Add(slot.EndTime);      // Kết thúc tại thời gian trong ngày

                var newSlot = new Slot
                {
                    ClassId = newClass.Id, // Gán slot cho lớp mới tạo
                    StartTime = startDateTime.TimeOfDay, // Chỉ lấy phần TimeSpan của thời gian
                    EndTime = endDateTime.TimeOfDay,     // Chỉ lấy phần TimeSpan của thời gian
                    Price = slot.Price, // Giữ nguyên giá slot
                    DayOfSlot = DateTime.Now.AddDays(i).DayOfWeek.ToString() // Ngày của slot
                };

                // Thêm slot vào cơ sở dữ liệu
                await _unitOfWork.SlotRepository.InsertAsync(newSlot);
            }

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


    }
}
