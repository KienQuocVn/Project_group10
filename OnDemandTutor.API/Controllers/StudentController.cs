using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Services; // Đảm bảo bạn có namespace đúng cho StudentService

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(IUnitOfWork unitOfWork)
    {
        _studentService = new StudentService(unitOfWork); // Tạo instance của StudentService
    }

    [HttpGet("export/{classId}")]
    public async Task<IActionResult> ExportStudentsToExcel(string classId)
    {
        try
        {
            byte[] excelFile = await _studentService.ExportStudentsToExcelAsync(classId);

            // Trả về file Excel dưới dạng file tải về
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Students_Class_{classId}.xlsx");
        }
        catch (Exception ex)
        {
            // Xử lý lỗi và trả về thông báo lỗi
            return BadRequest(new { message = ex.Message });
        }
    }
}
