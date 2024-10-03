using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.Core.Utils;
using OnDemandTutor.ModelViews.AuthModelViews;
using OnDemandTutor.ModelViews.UserModelViews;

using OnDemandTutor.Repositories.Entity;

using System.Security.Claims;

using IEmailSender = OnDemandTutor.Contract.Services.Interface.IEmailSender;

namespace OnDemandTutor.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Accounts> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;

        private static readonly Dictionary<string, (string Otp, DateTime Expiration)> OtpStore = new Dictionary<string, (string, DateTime)>();


        public UserService(IUnitOfWork unitOfWork, UserManager<Accounts> userManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender
          )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
          


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

            // Kiểm tra xem username đã tồn tại chưa
            var user = await accountRepositoryCheck.Entities.FirstOrDefaultAsync(x => x.UserName == model.Username);
            if (user != null)
            {
                throw new Exception("Duplicate");
            }

            // Kiểm tra xem giới tính có hợp lệ không
            var validGenders = new List<string> { "Male", "Female", "LGBT" };
            if (!validGenders.Contains(model.Gender))
            {
                throw new Exception("Invalid Gender");
            }

            var accountRepository = _unitOfWork.GetRepository<Accounts>();
            await accountRepository.InsertAsync(newAccount);
            await _unitOfWork.SaveAsync();

            // Sau khi tài khoản được tạo thành công, thêm vai trò mặc định "User"
            var roleRepository = _unitOfWork.GetRepository<ApplicationRole>();
            var userRole = await roleRepository.Entities.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                throw new Exception("The 'User' role does not exist. Please make sure to create it first.");
            }

            var userRoleRepository = _unitOfWork.GetRepository<ApplicationUserRoles>();
            var applicationUserRole = new ApplicationUserRoles
            {
                UserId = newAccount.Id,    // ID của tài khoản vừa tạo
                RoleId = userRole.Id,      // ID của vai trò "User"
                CreatedBy = model.Username,
                CreatedTime = DateTime.UtcNow,
                LastUpdatedBy = model.Username,
                LastUpdatedTime = DateTime.UtcNow
            };

            // Lưu vai trò mặc định "User" cho tài khoản mới
            await userRoleRepository.InsertAsync(applicationUserRole);
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

            if (role == null || role.Name == "User")
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
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                  
                    return false; // Trả về false nếu email không hợp lệ
                }

               

                // Tìm kiếm người dùng theo email (không phân biệt chữ hoa chữ thường)
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (user == null)
                {
                   
                    return false; // Trả về false nếu không tìm thấy người dùng
                }

                // Tạo OTP
                var random = new Random();
                string otp = random.Next(100000, 999999).ToString();

                // Thiết lập thời gian hết hạn OTP
                var expirationTime = DateTime.UtcNow.AddMinutes(10);

                // Lưu OTP
                if (OtpStore.ContainsKey(email))
                {
                    OtpStore[email] = (otp, expirationTime);
                }
                else
                {
                    OtpStore.Add(email, (otp, expirationTime));
                }

                // Gửi email OTP
                await _emailSender.SendOtpEmailAsync(email, otp);
              

                return true; // Trả về true nếu mọi thứ thành công
            }
            catch (Exception ex)
            {
              
                return false; // Trả về false nếu có ngoại lệ
            }
        }





        // Function to reset the password using the OTP
        public async Task<bool> ResetPasswordAsync(string email, string otp, string newPassword)
        {
            try
            {
              
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp) || string.IsNullOrWhiteSpace(newPassword))
                {
                  
                    return false; // Trả về false nếu dữ liệu không hợp lệ
                }

                // Tìm người dùng theo email
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (user == null)
                {
                   
                    return false; // Trả về false nếu không tìm thấy người dùng
                }

                // Kiểm tra tính hợp lệ của OTP
                if (!OtpStore.ContainsKey(email) || OtpStore[email].Otp != otp || OtpStore[email].Expiration < DateTime.UtcNow)
                {
                 
                    return false; // Trả về false nếu OTP không hợp lệ
                }

                // Sử dụng UserManager để đặt lại mật khẩu
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!result.Succeeded)
                {
                  
                    return false; // Trả về false nếu việc đặt lại mật khẩu không thành công
                }

                // Xóa OTP khỏi hệ thống sau khi sử dụng
                OtpStore.Remove(email);
                

                return true;
            }
            catch (Exception ex)
            {
              
                return false; // Trả về false khi có lỗi không mong đợi
            }

        }

            public async Task<bool> VerifyOtpAsync(string email, string otp)
            {
                try
                {
                    // Kiểm tra email và OTP có được cung cấp hay không
                    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
                    {
                       
                        return false;
                    }

                    // Kiểm tra xem OTP có tồn tại trong hệ thống không
                    if (!OtpStore.ContainsKey(email))
                    {
                        return false;
                    }

                    var storedOtp = OtpStore[email];

                   
                    // Kiểm tra tính hợp lệ của OTP
                    if (storedOtp.Otp != otp || storedOtp.Expiration < DateTime.UtcNow)
                    {
                        
                        return false;
                    }

                    // Nếu OTP hợp lệ, trả về true
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        // lấy danh sách account theo roleName
        public async Task<BasePaginatedList<Accounts>> GetAccountsByRoleAsync(string roleName, int pageNumber, int pageSize)
        {

            // Kiểm tra tham số roleName có tồn tại không
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new Exception("RoleName is empty.");

            }

            // Kiểm tra vai trò có tồn tại không
            ApplicationRole role = await _unitOfWork.GetRepository<ApplicationRole>().Entities.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null || role.Name == "User")
            {
                throw new Exception("Role does not exist");
            }

            // Lấy danh sách các ApplicationUserRoles liên kết với Role
            List<ApplicationUserRoles> userRoles = await _unitOfWork.GetRepository<ApplicationUserRoles>().Entities
                .Where(ur => ur.RoleId == role.Id && !ur.DeletedTime.HasValue)
                .ToListAsync();

            // Lấy danh sách AccountId từ ApplicationUserRoles
            List<Guid> accountIds = userRoles.Select(ur => ur.UserId).Distinct().ToList();

            // Lấy danh sách Accounts tương ứng với AccountId và áp dụng phân trang
            IQueryable<Accounts> accountsQuery = _unitOfWork.GetRepository<Accounts>().Entities
                .Where(a => accountIds.Contains(a.Id) && !a.DeletedTime.HasValue)
                .OrderBy(a => a.UserName);

            int totalCount = await accountsQuery.CountAsync();

            List<Accounts> paginatedAccounts = await accountsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new BasePaginatedList<Accounts>(paginatedAccounts, totalCount, pageNumber, pageSize);
        }


    }


}




