using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.Booking;
using OnDemandTutor.Repositories.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using OnDemandTutor.Services.Service.AccountUltil;
using OnDemandTutor.Repositories.UOW; // Sửa thành đúng namespace



namespace OnDemandTutor.Services.Service

{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountUtils _accountUtil;  // Sửa lại thành AccountUtils
        private readonly AuthenticationRepository _authenticationRepository;

        public BookingService(IUnitOfWork unitOfWork, AccountUtils accountUtil, AuthenticationRepository authenticationRepository)  // Sửa lại thành AccountUtils
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _accountUtil = accountUtil ?? throw new ArgumentNullException(nameof(accountUtil));
            _authenticationRepository = authenticationRepository;
        }


        public async Task<string> BookSubjectBySlot(SlotBookingDto dto)
        {
            Accounts accounts = _accountUtil.GetCurrentUser();

            if (dto == null)
                return "Booking data is null";

            var student = await _unitOfWork.GetRepository<Accounts>().FindAsync(dto.StudentId);
            if (student == null)
                return "Student not found";

            var subject = await _unitOfWork.SubjectRepository.FindAsync(dto.SubjectId);
            if (subject == null)
                return "Subject not found";

            var tutorSubject = await _unitOfWork.TutorRepository.Entities
                .FirstOrDefaultAsync(ts => ts.TutorId == dto.TutorSubjectId);
            if (tutorSubject == null)
                return "Tutor for this subject not found";

            var selectedSlot = await _unitOfWork.SlotRepository.FindAsync(dto.SlotId);
            if (selectedSlot == null)
                return "Selected slot not found";

            // Kiểm tra nếu slot đã được đặt trước
            var existingBookings = await _unitOfWork.GetRepository<Booking>().Entities
                .Where(b => b.SlotId == selectedSlot.Id.ToString() && b.BookingDate.Date == dto.SelectedDate.Date)
                .ToListAsync();

            if (existingBookings.Any())
            {
                return "The selected slot is already booked.";
            }

            double durationInHours = (selectedSlot.EndTime - selectedSlot.StartTime).TotalHours;
            double totalPrice = selectedSlot.Price * durationInHours;

            if (accounts.UserInfo.Balance >= totalPrice)
            {
                accounts.UserInfo.Balance -= totalPrice;
                _authenticationRepository.Update(accounts);

                var newClass = new Class
                {
                    AccountId = tutorSubject.TutorId,
                    SubjectId = dto.SubjectId.ToString(),
                    AmountOfSlot = 20,
                    StartDay = DateTime.UtcNow,
                    EndDay = DateTime.UtcNow.AddMonths(1),
                };

                await _unitOfWork.ClassRepository.InsertAsync(newClass);
                await _unitOfWork.SaveAsync();

                for (int i = 0; i < 20; i++)
                {
                    var startDateTime = DateTime.Now.Date.AddDays(i).Add(selectedSlot.StartTime);
                    var endDateTime = DateTime.Now.Date.AddDays(i).Add(selectedSlot.EndTime);

                    var newSlot = new Slot
                    {
                        ClassId = newClass.Id,
                        StartTime = startDateTime.TimeOfDay,
                        EndTime = endDateTime.TimeOfDay,
                        Price = selectedSlot.Price,
                        DayOfSlot = DateTime.Now.AddDays(i).DayOfWeek.ToString()
                    };

                    await _unitOfWork.SlotRepository.InsertAsync(newSlot);
                }

                await _unitOfWork.SaveAsync();

                var booking = new Booking
                {
                    StudentId = dto.StudentId,
                    SubjectId = dto.SubjectId,
                    TutorId = tutorSubject.TutorId,
                    UserId = tutorSubject.UserId,
                    SlotId = selectedSlot.Id.ToString(),
                    BookingDate = DateTime.UtcNow,
                    TotalPrice = selectedSlot.Price,
                    StartTime = selectedSlot.StartTime,
                    EndTime = selectedSlot.EndTime
                };

                await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
                await _unitOfWork.SaveAsync();

                return $"Subject booked successfully. Total Price: {selectedSlot.Price:C}";
            }
            else
            {
                return "Insufficient balance.";
            }
        }

        public async Task<string> BookSubjectByTime(TimeBookingDto dto)
        {

            Accounts accounts = _accountUtil.GetCurrentUser();
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
            if (accounts.UserInfo.Balance >= totalPrice)
            {
                accounts.UserInfo.Balance -= totalPrice;
                _authenticationRepository.Update(accounts);
                // Đặt booking cho môn học này
                var booking = new Booking
                {
                    StudentId = dto.StudentId,
                    SubjectId = dto.SubjectId,
                    TutorId = tutorSubject.TutorId,
                    SlotId = dto.SlotId,
                    BookingDate = DateTime.UtcNow,
                    TotalPrice = totalPrice,
                    StartTime = dto.StartTime, // Thời gian bắt đầu
                    EndTime = dto.EndTime // Thời gian kết thúc
                };

                // Thêm booking vào cơ sở dữ liệu  
                await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);

                // Lưu các thay đổi vào cơ sở dữ liệu  
                await _unitOfWork.SaveAsync();

                // Trả về thông báo thành công với tổng giá
                return $"Subject booked successfully and class has been scheduled. Total Price: {totalPrice:C}";
            }
            else { return $"Booking fail"; }
        }

        // Kiểm tra xem thời gian có khả dụng không
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

        public async Task<string> CancelBooking(Guid bookingId, string deletedBy)
        {
            // Tìm booking theo ID
            var booking = await _unitOfWork.GetRepository<Booking>().FindAsync(bookingId);

            if (booking == null || booking.IsDeleted)
            {
                return "Booking not found or already canceled.";
            }

            // Lấy thông tin sinh viên và kiểm tra tài khoản
            var student = await _unitOfWork.GetRepository<Accounts>().FindAsync(booking.StudentId);
            if (student == null)
            {
                return "Student not found.";
            }

            // Hoàn tiền lại cho sinh viên (nếu cần)
            var slot = await _unitOfWork.GetRepository<Slot>().FindAsync(booking.SlotId);
            if (slot == null)
            {
                return "Slot not found.";
            }

            double durationInHours = (slot.EndTime - slot.StartTime).TotalHours;
            double totalPrice = slot.Price * durationInHours;

            // Kiểm tra nếu có hoàn tiền
            if (student.UserInfo.Balance != null)
            {
                student.UserInfo.Balance += totalPrice;
                _authenticationRepository.Update(student);
            }

            // Đánh dấu booking là đã bị hủy bằng cách cập nhật thời gian xóa mềm
            booking.DeletedTime = DateTimeOffset.UtcNow;
            booking.DeletedBy = deletedBy;

            // Cập nhật trạng thái booking
            _unitOfWork.GetRepository<Booking>().Update(booking);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveAsync();

            return "Booking canceled successfully and refund processed.";
        }




    }
}
