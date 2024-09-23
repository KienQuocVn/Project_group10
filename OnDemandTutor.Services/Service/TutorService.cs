using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.TutorSubjectModelViews;
using OnDemandTutor.Repositories.Entity;


namespace OnDemandTutor.Services.Service
{
    public class TutorService : ITutorService
    {
        // Khai báo biến _unitOfWork để truy cập các repository  
        private readonly IUnitOfWork _unitOfWork;

        // Constructor nhận vào IUnitOfWork để khởi tạo service  
        public TutorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // Gán giá trị cho biến _unitOfWork  
        }

        // Phương thức lấy danh sách tất cả các môn học của gia sư với phân trang  
        public async Task<BasePaginatedList<TutorSubject>> GetAllTutorSubjectsAsync(int pageNumber, int pageSize)
        {
            try
            {
                // Truy vấn để lấy danh sách môn học chưa bị xóa, sắp xếp theo thời gian tạo giảm dần  
                var tutorSubjectQuery = _unitOfWork.GetRepository<TutorSubject>().Entities
                 .Where(p => !p.DeletedTime.HasValue) // Lọc các môn học chưa bị xóa  
                 .OrderByDescending(p => p.CreatedTime); // Sắp xếp theo thời gian tạo  

                // Đếm tổng số môn học  
                int totalCount = await tutorSubjectQuery.CountAsync();

                // Lấy danh sách môn học với phân trang  
                var tutorSubjects = await tutorSubjectQuery
                    .Skip((pageNumber - 1) * pageSize) // Bỏ qua số lượng môn học theo trang  
                    .Take(pageSize) // Lấy số lượng môn học theo kích thước trang  
                    .ToListAsync();

                // Ghi log thông báo thành công  
                Console.WriteLine("Lấy danh sách môn học của gia sư thành công.");

                // Trả về danh sách môn học với thông tin phân trang  
                return new BasePaginatedList<TutorSubject>(tutorSubjects, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu có  
                Console.WriteLine($"Lỗi khi lấy danh sách môn học của gia sư: {ex.Message}");

                // Ném ra một ngoại lệ tùy chỉnh với thông tin lỗi  
                throw new Exception("Lỗi khi lấy danh sách môn học của gia sư.", ex);
            }
        }

        public async Task<BasePaginatedList<TutorSubject>> SearchTutorSubjectsByNameAsync(string subjectName, int pageNumber, int pageSize)
        {
            // Lấy danh sách Subject có tên chứa subjectName  
            var subjectIds = await _unitOfWork.SubjectRepository.Entities
                .Where(s => s.Name.Contains(subjectName))
                .Select(s => s.Id)
                .ToListAsync();

            if (!subjectIds.Any())
            {
                return new BasePaginatedList<TutorSubject>(new List<TutorSubject>(), 0, pageNumber, pageSize);
            }

            // Lấy tất cả TutorSubjects  
            var allTutorSubjects = await _unitOfWork.TutorRepository.GetAllAsync();

            // Lọc TutorSubjects theo SubjectId  
            var filteredTutorSubjects = allTutorSubjects
                .Where(ts => subjectIds.Contains(ts.SubjectId))
                .AsQueryable();

            return await _unitOfWork.TutorRepository.GetPagging(filteredTutorSubjects, pageNumber, pageSize);
        }

        // Phương thức lấy thông tin môn học của gia sư theo ID của gia sư và môn học  
        public async Task<TutorSubject> GetByTutorIdSubjectIdAsync(Guid tutorId, string subjectId)
        {
            try
            {
                // Gọi phương thức để lấy môn học theo ID của gia sư và môn học  
                var tutorSubject = await _unitOfWork.TutorRepository.GetByTutorIdSubjectIdAsync(tutorId, subjectId);

                // Kiểm tra kết quả và ghi log thành công nếu tìm thấy  
                if (tutorSubject != null)
                {
                    Console.WriteLine($"Lấy thông tin môn học của gia sư thành công: TutorId = {tutorId}, SubjectId = {subjectId}");
                }
                else
                {
                    // Ghi log nếu không tìm thấy môn học  
                    Console.WriteLine($"Không tìm thấy thông tin môn học cho gia sư: TutorId = {tutorId}, SubjectId = {subjectId}");
                }

                // Trả về môn học tìm thấy (có thể null nếu không tìm thấy)  
                return tutorSubject;
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi nếu có  
                Console.WriteLine($"Lỗi khi lấy thông tin môn học của gia sư: {ex.Message}");

                // Ném ra một ngoại lệ tùy chỉnh với thông tin lỗi  
                throw new Exception("Lỗi khi lấy thông tin môn học của gia sư.", ex);
            }
        }

        public async Task<TutorSubject> CreateTutorSubjectAsync(CreateTutorSubjectModelViews model)
        {
            try
            {
                var _account = new Accounts
                {

                };

                // Tạo một đối tượng TutorSubject mới với thông tin từ model  
                var tutorSubject = new TutorSubject
                {
                    Tutor = _account,
                    Id = Guid.NewGuid(), // Tạo ID mới cho môn học  
                    TutorId = model.TutorId, // Gán ID của gia sư từ model  
                    SubjectId = model.SubjectId, // Gán ID của môn học từ model  
                    CreatedTime = DateTimeOffset.Now, // Ghi lại thời gian tạo  
                    LastUpdatedTime = DateTimeOffset.Now // Ghi lại thời gian cập nhật lần cuối  
                };

                // Thêm đối tượng TutorSubject vào cơ sở dữ liệu  
                await _unitOfWork.TutorRepository.InsertAsync(tutorSubject);
                // Lưu thay đổi vào cơ sở dữ liệu  
                await _unitOfWork.SaveAsync();

                // Ghi log thông báo thành công  
                Console.WriteLine($"Tạo mới môn học của gia sư thành công: TutorId = {model.TutorId}, SubjectId = {model.SubjectId}");

                // Trả về đối tượng TutorSubject vừa tạo  
                return tutorSubject;
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi nếu có  
                Console.WriteLine($"Lỗi khi tạo mới môn học của gia sư: {ex.Message}");
                // Ném lại ngoại lệ với thông điệp rõ ràng  
                throw new Exception("Không thể tạo mới môn học của gia sư.", ex);
            }
        }

        public async Task<TutorSubject> UpdateTutorSubjectAsync(Guid tutorId, string subjectId, UpdateTutorSubjectModelViews model)
        {
            try
            {
                // Lấy thông tin môn học hiện tại dựa trên ID của gia sư và môn học  
                var existingTutorSubject = await _unitOfWork.TutorRepository.GetByTutorIdSubjectIdAsync(tutorId, subjectId);
                if (existingTutorSubject != null)
                {
                    // Cập nhật thời gian cập nhật  
                    existingTutorSubject.LastUpdatedTime = DateTimeOffset.Now;
                    // Cập nhật các trường khác từ model nếu cần (có thể thêm logic ở đây)  

                    // Cập nhật thông tin môn học trong cơ sở dữ liệu  
                    _unitOfWork.TutorRepository.Update(existingTutorSubject);
                    await _unitOfWork.SaveAsync();

                    // Ghi log thông báo thành công  
                    Console.WriteLine($"Cập nhật môn học của gia sư thành công: TutorId = {tutorId}, SubjectId = {subjectId}");

                    // Trả về đối tượng TutorSubject đã cập nhật  
                    return existingTutorSubject;
                }

                // Ghi log thông báo không tìm thấy môn học  
                Console.WriteLine($"Không tìm thấy môn học của gia sư: TutorId = {tutorId}, SubjectId = {subjectId}");
                return null; // Trả về null nếu không tìm thấy  
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi nếu có  
                Console.WriteLine($"Lỗi khi cập nhật môn học của gia sư: {ex.Message}");
                // Ném lại ngoại lệ với thông điệp rõ ràng  
                throw new Exception("Không thể cập nhật môn học của gia sư.", ex);
            }
        }

        public async Task<bool> DeleteTutorSubjectByTutorIdAndSubjectIdAsync(Guid tutorId, string subjectId)
        {
            try
            {
                // Lấy thông tin môn học hiện tại dựa trên ID của gia sư và môn học  
                var existingTutorSubject = await _unitOfWork.TutorRepository.Entities
                    .FirstOrDefaultAsync(ts => ts.TutorId == tutorId && ts.SubjectId == subjectId);

                if (existingTutorSubject != null)
                {
                    // Đánh dấu môn học là đã bị xóa bằng cách cập nhật thời gian xóa  
                    existingTutorSubject.DeletedTime = DateTimeOffset.Now;

                    // Cập nhật trạng thái xóa trong cơ sở dữ liệu  
                    _unitOfWork.TutorRepository.Update(existingTutorSubject);
                    await _unitOfWork.SaveAsync();

                    // Ghi log thông báo thành công  
                    Console.WriteLine($"Xóa môn học của gia sư thành công: TutorId = {tutorId}, SubjectId = {subjectId}");
                    return true; // Trả về true nếu xóa thành công  
                }

                // Ghi log thông báo không tìm thấy môn học để xóa  
                Console.WriteLine($"Không tìm thấy môn học của gia sư để xóa: TutorId = {tutorId}, SubjectId = {subjectId}");
                return false; // Trả về false nếu không tìm thấy môn học  
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết lỗi nếu có  
                Console.WriteLine($"Lỗi khi xóa môn học của gia sư: {ex.Message}");
                // Ném lại ngoại lệ với thông điệp rõ ràng  
                throw new Exception("Không thể xóa môn học của gia sư.", ex);
            }
        }

        
    }
}