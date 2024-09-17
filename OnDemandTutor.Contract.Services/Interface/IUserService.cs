using OnDemandTutor.ModelViews.AuthModelViews;
using OnDemandTutor.ModelViews.UserModelViews;
using OnDemandTutor.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Contract.Services.Interface
{
    public interface IUserService
    {
        // Lấy danh sách tất cả người dùng
        Task<IEnumerable<UserResponseModel>> GetAllUsersAsync();

        // Tìm người dùng theo ID
        Task<UserResponseModel> GetUserByIdAsync(string userId);

        // Tạo tài khoản người dùng mới
        Task<Accounts> CreateAccountAsync(CreateAccountModel model);

        // Cập nhật thông tin người dùng
        Task<bool> UpdateUserAsync(string userId, UpdateUserModel model);

        // Xóa người dùng
        Task<bool> DeleteUserAsync(string userId);

        // Xác thực người dùng (Đăng nhập)
        Task<Accounts> AuthenticateAsync(LoginModelView model);

        Task<bool> AddRoleToAccountAsync(Guid userId, string roleName);
    }
}
