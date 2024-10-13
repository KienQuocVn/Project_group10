using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Contract.Repositories.IUOW
{
    public interface IAuthenticationRepository
    {
        Accounts FindByEmail(string email);
    }
}
