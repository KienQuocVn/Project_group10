using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.ModelViews.AuthModelViews;
using OnDemandTutor.ModelViews.UserModelViews;
using OnDemandTutor.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IList<UserResponseModel>> GetAll()
        {
            IList<UserResponseModel> users = new List<UserResponseModel>
            {
                new UserResponseModel { Id = "1" },
                new UserResponseModel { Id = "2" },
                new UserResponseModel { Id = "3" }
            };
            return Task.FromResult(users);
        }
        //public async Task<Accounts> CreateAccountAsync(CreateAccountModel model)
        //{
        //    // Hash password
        //    var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        //    // Create new account
        //    var newAccount = new Accounts
        //    {
        //        UserName = model.Username,
        //        Email = model.Email,
        //        PasswordHash = passwordHash
        //    };

        //    // Sử dụng UnitOfWork để thêm tài khoản mới
        //    await _unitOfWork.Accounts.AddAsync(newAccount);

        //    // Save tất cả thay đổi thông qua UnitOfWork
        //    await _unitOfWork.SaveAsync();

        //    return newAccount;
        //}
    }
}
