using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Services;
using OnDemandTutor.Services.Service;
using System.Threading.Tasks;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportStudentController : ControllerBase
    {
        private readonly ExportStudent _exportStudentService;

        public ExportStudentController(IUnitOfWork unitOfWork)
        {
            _exportStudentService = new ExportStudent(unitOfWork); // Tạo instance của StudentService
        }

        [HttpGet("export/{classId}")]
        public async Task<IActionResult> ExportStudentsToExcel(string classId)
        {
            try
            {
                byte[] excelFile = await _exportStudentService.ExportStudentsToExcelAsync(classId);

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
}
