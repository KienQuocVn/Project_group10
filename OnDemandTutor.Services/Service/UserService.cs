using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Utils;
using OnDemandTutor.ModelViews.AuthModelViews;
using OnDemandTutor.ModelViews.UserModelViews;
using OnDemandTutor.Repositories.Context;
using OnDemandTutor.Repositories.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Accounts> _userManager;


        public UserService(IUnitOfWork unitOfWork, UserManager<Accounts> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            
        }

        // Lấy danh sách tất cả người dùng
        public async Task<IEnumerable<UserResponseModel>> GetAllUsersAsync()
        {
            var userRepository = _unitOfWork.GetRepository<Accounts>();
            var users = await userRepository.GetAllAsync();

            // Mapping dữ liệu từ Accounts sang UserResponseModel
            var userResponses = new List<UserResponseModel>();
            foreach (var user in users)
            {
                userResponses.Add(new UserResponseModel
                {
                    
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.UserInfo?.FullName,
                    Gender = user.UserInfo?.Gender
                });
            }

            return userResponses;
        }

        // Tìm người dùng theo ID
        public async Task<UserResponseModel> GetUserByIdAsync(string userId)
        {
            var userRepository = _unitOfWork.GetRepository<Accounts>();
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null) return null;

            return new UserResponseModel
            {
              
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.UserInfo?.FullName,
                Gender = user.UserInfo?.Gender
            };
        }

        // Tạo tài khoản người dùng mới
        public async Task<Accounts> CreateAccountAsync(CreateAccountModel model)
        {
            var userInfo = new UserInfo
            {
                FullName = model.FullName,
                Gender = model.Gender,
                LinkCV = model.linkCV
                
            };

            var newAccount = new Accounts
            {
                UserName = model.Username,
                Email = model.Email,
                PasswordHash = model.Password,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserInfo = userInfo
               
            };
            
            var accountRepositoryCheck = _unitOfWork.GetRepository<Accounts>();

            var user = await accountRepositoryCheck.Entities
                .FirstOrDefaultAsync(x => x.UserName == model.Username);
            if (user != null) {

               throw new Exception("Duplicate");
            }

            var check = await accountRepositoryCheck.Entities.FirstOrDefaultAsync(x => x.UserInfo.Gender == "Male" || x.UserInfo.Gender == "FeMale" || x.UserInfo.Gender == "LGPT");
            if (check != null) {
                throw new Exception("Gender");
            }

            var accountRepository = _unitOfWork.GetRepository<Accounts>();
            await accountRepository.InsertAsync(newAccount);

            await _unitOfWork.SaveAsync();

            return newAccount;
        }

        // Cập nhật thông tin người dùng
        public async Task<bool> UpdateUserAsync(string userId, UpdateUserModel model)
        {
            var userRepository = _unitOfWork.GetRepository<Accounts>();
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null) return false;

            user.UserName = model.Username;
            user.Email = model.Email;

            if (user.UserInfo != null)
            {
                user.UserInfo.FullName = model.FullName;
                user.UserInfo.Gender = model.Gender;
            }

            await userRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return true;
        }

        // Xóa người dùng
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var userRepository = _unitOfWork.GetRepository<Accounts>();
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null) return false;

            await userRepository.DeleteAsync(userId);
            await _unitOfWork.SaveAsync();

            return true;
        }

        // Xác thực người dùng (đăng nhập)
        public async Task<Accounts> AuthenticateAsync(LoginModelView model)
        {
            var accountRepository = _unitOfWork.GetRepository<Accounts>();

            var user = await accountRepository.Entities
                .FirstOrDefaultAsync(x => x.UserName == model.Username);

            if (user == null)
            {
                return null; 
            }


            if (model.Password != user.PasswordHash)
            {
                return null; 
            }
            var loginInfo = new ApplicationUserLogins
            {
                UserId = user.Id, // UserId từ người dùng đã đăng nhập
                ProviderKey = user.Id.ToString(),
                LoginProvider = "CustomLoginProvider", // Hoặc có thể là tên provider khác
                ProviderDisplayName = "Standard Login",
                CreatedBy = user.UserName, // Ghi lại ai đã thực hiện đăng nhập
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedBy = user.UserName,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };
            var loginRepository = _unitOfWork.GetRepository<ApplicationUserLogins>();
            await loginRepository.InsertAsync(loginInfo);
            await _unitOfWork.SaveAsync();


            return user;
        }
        public async Task<bool> AddRoleToAccountAsync(Guid userId, string roleName)
        {
            // Tìm tài khoản người dùng đã tồn tại
            var accountRepository = _unitOfWork.GetRepository<Accounts>();
            var user = await accountRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Kiểm tra vai trò có tồn tại không
            var roleRepository = _unitOfWork.GetRepository<ApplicationRole>();
            var role = await roleRepository.Entities.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
            {
                throw new Exception("Role does not exist");
            }

            // Kiểm tra nếu người dùng đã có vai trò này
            var userRoleRepository = _unitOfWork.GetRepository<ApplicationUserRoles>();
            var existingUserRole = await userRoleRepository.Entities
                .AsNoTracking()  // Không theo dõi thực thể này
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);

            if (existingUserRole != null)
            {
                throw new Exception("User already has this role");
            }

            // Nếu không tồn tại, thêm vai trò cho người dùng
            var applicationUserRole = new ApplicationUserRoles
            {
                UserId = user.Id,
                RoleId = role.Id,
                CreatedBy = user.UserName,  // Ghi lại ai đã thêm vai trò này
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedBy = user.UserName,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };

            // Lưu thông tin vào ApplicationUserRoles
            await userRoleRepository.InsertAsync(applicationUserRole);
            await _unitOfWork.SaveAsync();

            return true;
        }



    }
}
