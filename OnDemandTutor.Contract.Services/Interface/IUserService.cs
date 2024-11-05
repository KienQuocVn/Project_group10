﻿using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Core.Base;
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

        Task<bool> AddRoleAsync(string roleName, string createdBy);


        Task<bool> AddClaimToRoleAsync(Guid roleId, string claimType, string claimValue, string createdBy);

        Task<bool> AddClaimToUserAsync(Guid userId, string claimType, string claimValue, string createdBy);
        Task<IEnumerable<ApplicationUserClaims>> GetUserClaimsAsync(Guid userId);
        Task<bool> UpdateClaimAsync(Guid claimId, string claimType, string claimValue, string updatedBy);
        Task<bool> SoftDeleteClaimAsync(Guid claimId, string deletedBy);

        Task<bool> ResetPasswordAsync(string email, string otp, string newPassword);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);

        Task<BasePaginatedList<Accounts>> GetAccountsByRoleAsync(string roleName, int pageNumber, int pageSize);
        Task<double> CalculateSalaryAsync(Guid userId, double commissionRate, DateTime? startDate = null, DateTime? endDate = null, string? subjectId = null);
        Task<double> CalculateMonthSalaryAsync(Guid tutorId, int month, int year);
        Task<int> ProcessMonthEndSalaryAsync();
    }
}
