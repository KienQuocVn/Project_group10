using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Context;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Repositories.UOW
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DatabaseContext _dbContext;

        public AuthenticationRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Accounts FindByEmail(string email)
        {
            return _dbContext.ApplicationUsers.FirstOrDefault(a => a.Email == email);
        }
    }
}
