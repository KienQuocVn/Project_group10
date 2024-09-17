using OnDemandTutor.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.Entity;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IClassService
    {
        Task<IList<Class>> GetAllClass();
        
        Task<Class> AddClass(Class model);
    }
}
