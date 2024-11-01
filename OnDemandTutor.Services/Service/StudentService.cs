using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Repositories.Entity;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> ExportStudentsToExcelAsync(string classId)
    {
        // Kiểm tra sự tồn tại của Class
        bool isExistClass = await _unitOfWork.GetRepository<Class>().Entities
            .AnyAsync(c => c.Id == classId && !c.DeletedTime.HasValue);

        if (!isExistClass)
        {
            throw new Exception("Không tìm thấy lớp học (Class) hoặc lớp học đã bị xóa!");
        }

        // Lấy danh sách sinh viên trong lớp học
        var studentsInClass = await _unitOfWork.GetRepository<Accounts>().Entities
            .Where(a => a.Classes.Any(c => c.Id == classId) && !a.DeletedTime.HasValue)
            .Include(a => a.UserInfo) // Nếu bạn muốn lấy thông tin người dùng kèm theo
            .ToListAsync();

        if (studentsInClass == null || !studentsInClass.Any())
        {
            throw new Exception("Không tìm thấy sinh viên trong lớp học này!");
        }

        // Tạo file Excel
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Students");

            // Thiết lập tiêu đề cột
            worksheet.Cell(1, 1).Value = "ID Sinh viên";
            worksheet.Cell(1, 2).Value = "Họ và tên";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Giới tính";
            worksheet.Cell(1, 5).Value = "Thời gian đăng ký";

            // Điền dữ liệu sinh viên
            for (int i = 0; i < studentsInClass.Count; i++)
            {
                var student = studentsInClass[i];
                worksheet.Cell(i + 2, 1).Value = student.Id; // ID của sinh viên
                worksheet.Cell(i + 2, 2).Value = student.UserInfo?.FullName; 
                worksheet.Cell(i + 2, 3).Value = student.Email; 
                worksheet.Cell(i + 2, 4).Value = student.UserInfo?.Gender; 
                worksheet.Cell(i + 2, 5).Value = student.CreatedTime; 
            }

            // Thiết lập định dạng
            worksheet.Columns().AdjustToContents();

            // Lưu file vào bộ nhớ
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray(); // Trả về byte array để sử dụng
            }
        }
    }

}
