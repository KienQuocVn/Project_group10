using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IStudentService
    {
        Task<byte[]> ExportStudentsToExcelAsync(string? classId);
    }
}
