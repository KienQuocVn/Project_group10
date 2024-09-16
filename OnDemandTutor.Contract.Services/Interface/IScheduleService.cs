using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IScheduleService
    {
        Task<BasePaginatedList<Schedule>> GetAllSchedulesAsync(int pageNumber, int pageSize);
        Task<Schedule> GetScheduleByIdAsync(string id);
        Task<Schedule> CreateScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(string id);
    }
}
