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
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnDemandTutor.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Accounts> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<Accounts> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;

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

            // Tìm người dùng theo Username
            var user = await accountRepository.Entities
                .FirstOrDefaultAsync(x => x.UserName == model.Username);

            if (user == null)
            {
                return null; // Người dùng không tồn tại
            }

            // So sánh mật khẩu (bạn có thể sử dụng cơ chế mã hóa mật khẩu)
            if (model.Password != user.PasswordHash)
            {
                return null; // Mật khẩu không khớp
            }

            // Kiểm tra xem đã tồn tại bản ghi đăng nhập chưa
            var loginRepository = _unitOfWork.GetRepository<ApplicationUserLogins>();
            var existingLogin = await loginRepository.Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == "CustomLoginProvider");

            if (existingLogin == null)
            {
                // Nếu chưa có bản ghi đăng nhập, thêm mới
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

                await loginRepository.InsertAsync(loginInfo);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }
            else
            {
                // Nếu bản ghi đăng nhập đã tồn tại, có thể cập nhật thông tin nếu cần
                existingLogin.LastUpdatedBy = user.UserName;
                existingLogin.LastUpdatedTime = CoreHelper.SystemTimeNow;

                await loginRepository.UpdateAsync(existingLogin);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return user; // Trả về người dùng đã xác thực
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
        public async Task<bool> AddRoleAsync(string roleName, string createdBy)
        {
            // Kiểm tra nếu role đã tồn tại
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return false; // Role đã tồn tại
            }

            // Tạo đối tượng ApplicationRole mới
            var role = new ApplicationRole
            {
                Name = roleName,
                CreatedBy = createdBy,
                CreatedTime = DateTimeOffset.Now,
                LastUpdatedBy = createdBy,
                LastUpdatedTime = DateTimeOffset.Now
            };

            // Lấy repository cho ApplicationRole
            var roleRepository = _unitOfWork.GetRepository<ApplicationRole>();

            // Thêm role vào database qua repository
            await roleRepository.InsertAsync(role);

            // Lưu thay đổi vào database
            await _unitOfWork.SaveAsync();

            return true; 
        }


        public async Task<bool> AddClaimToRoleAsync(Guid roleId, string claimType, string claimValue, string createdBy)
        {
            // Tìm role theo roleId
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false; // Role không tồn tại
            }

            // Tạo một claim mới
            var claim = new Claim(claimType, claimValue);

            // Thêm claim cho role
            var result = await _roleManager.AddClaimAsync(role, claim);
            if (result.Succeeded)
            {
                // Tạo bản ghi mới trong ApplicationRoleClaims
                var roleClaim = new ApplicationRoleClaims
                {
                    RoleId = roleId,
                    ClaimType = claimType,
                    ClaimValue = claimValue,
                    CreatedBy = createdBy,
                    CreatedTime = DateTimeOffset.Now,
                    LastUpdatedBy = createdBy,
                    LastUpdatedTime = DateTimeOffset.Now
                };

                var roleClaimRepository = _unitOfWork.GetRepository<ApplicationRoleClaims>();
                await roleClaimRepository.InsertAsync(roleClaim);

                // Lưu thay đổi
                await _unitOfWork.SaveAsync();

                return true;
            }

            return false; // Thêm claim thất bại
        }
        // Thêm claim mới cho người dùng
        public async Task<bool> AddClaimToUserAsync(Guid userId, string claimType, string claimValue, string createdBy)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            var claim = new ApplicationUserClaims
            {
                UserId = userId,
                ClaimType = claimType,
                ClaimValue = claimValue,
                CreatedBy = createdBy,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedBy = createdBy,
                LastUpdatedTime = CoreHelper.SystemTimeNow
            };

            var claimRepository = _unitOfWork.GetRepository<ApplicationUserClaims>();
            await claimRepository.InsertAsync(claim);
            await _unitOfWork.SaveAsync();

            return true;
        }

        // Lấy danh sách các claim của người dùng
        public async Task<IEnumerable<ApplicationUserClaims>> GetUserClaimsAsync(Guid userId)
        {
            var claimRepository = _unitOfWork.GetRepository<ApplicationUserClaims>();
            var claims = await claimRepository.Entities
                .Where(c => c.UserId == userId && c.DeletedTime == null) // Không lấy claim đã bị xóa
                .ToListAsync();

            return claims;
        }

        // Cập nhật thông tin của claim
        public async Task<bool> UpdateClaimAsync(Guid claimId, string claimType, string claimValue, string updatedBy)
        {
            var claimRepository = _unitOfWork.GetRepository<ApplicationUserClaims>();
            var claim = await claimRepository.GetByIdAsync(claimId);

            if (claim == null)
            {
                return false; // Claim không tồn tại
            }

            // Cập nhật thông tin claim
            claim.ClaimType = claimType;
            claim.ClaimValue = claimValue;
            claim.LastUpdatedBy = updatedBy;
            claim.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await claimRepository.UpdateAsync(claim);
            await _unitOfWork.SaveAsync();

            return true;
        }

        // Xóa mềm claim
        public async Task<bool> SoftDeleteClaimAsync(Guid claimId, string deletedBy)
        {
            var claimRepository = _unitOfWork.GetRepository<ApplicationUserClaims>();
            var claim = await claimRepository.GetByIdAsync(claimId);

            if (claim == null)
            {
                return false; // Claim không tồn tại
            }

            // Đánh dấu claim là đã xóa
            claim.DeletedBy = deletedBy;
            claim.DeletedTime = CoreHelper.SystemTimeNow;

            await claimRepository.UpdateAsync(claim);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }



}

