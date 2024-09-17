using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.Entity;

namespace OnDemandTutor.Services.Service
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IList<Class>> GetAllClass()
        {
            return await _unitOfWork.GetRepository<Class>().GetAllAsync();
        }

        public async Task<Class> AddClass(Class entity)
        {
            return  entity;
        }
    }
}
