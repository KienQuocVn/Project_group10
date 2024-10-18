using System;
using System.Linq;
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

        public void Update(Accounts accounts)
        {
            // Check if the account exists in the database
            var existingAccount = _dbContext.ApplicationUsers.FirstOrDefault(a => a.Id == accounts.Id);
            if (existingAccount != null)
            {
                // Update properties
                existingAccount.UserInfo.Balance = accounts.UserInfo.Balance;
                // Update other properties as needed

                // Save changes to the database
                _dbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Account not found");
            }
        }
    }
}
