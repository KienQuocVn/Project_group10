﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Core.Base;
using OnDemandTutor.ModelViews.UserModelViews;
using OnDemandTutor.Services.Service;

namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserService> _logger;
        public UsersController(IUserService userService, ILogger<UserService> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                // Gọi phương thức ForgotPasswordAsync
                var result = await _userService.ForgotPasswordAsync(email);

                if (result)
                {
                    return Ok("bạn sẽ nhận được một email đặt lại mật khẩu.");
                }
                else
                {
                    // Nếu không thành công, trả về thông báo lỗi
                    return BadRequest("Không tìm thấy người dùng với email này hoặc đã xảy ra lỗi. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                // Ghi log và trả về lỗi 500 nếu có lỗi không mong đợi
                _logger.LogError(ex, "Lỗi xử lý");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý yêu cầu của bạn. Vui lòng thử lại sau.");
            }
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string otp, [FromQuery] string newPassword)
        {
            try
            {
                var result = await _userService.ResetPasswordAsync(email, otp, newPassword);

                if (result)
                {
                    return Ok("Mật khẩu đã được đặt lại thành công.");
                }
                else
                {
                    return BadRequest("Email không tồn tại, OTP không hợp lệ hoặc mật khẩu mới không đáp ứng yêu cầu. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi xử lý yêu cầu đặt lại mật khẩu.");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý yêu cầu của bạn. Vui lòng thử lại sau.");
            }
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string email, [FromQuery] string otp)
        {
            try
            {
                var result = await _userService.VerifyOtpAsync(email, otp);

                if (result)
                {
                    return Ok("OTP hợp lệ.");
                }
                else
                {
                    return BadRequest("OTP không hợp lệ hoặc đã hết hạn.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi xử lý yêu cầu kiểm tra OTP.");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý yêu cầu của bạn. Vui lòng thử lại sau.");
            }
        }


        // GET: api/GetSalaryByUserID
        // Lấy lương theo id gia sư với các bộ lọc tùy chọn
        [HttpGet("GetSalaryByUserID")]
        public async Task<IActionResult> GetSalaryByUserID(
            Guid userId,
            [FromQuery] double commissionRate = 0.1,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? subjectId = null)
        {
            try
            {
                // Gọi service với các tham số tìm kiếm
                var result = await _userService.CalculateSalaryAsync(userId, commissionRate, startDate, endDate, subjectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        // GET: api/User/CalculateMonthSalary
        [HttpGet("CalculateMonthSalary")]
        public async Task<IActionResult> CalculateMonthSalary(
            Guid tutorId,
            [FromQuery] int? month = null,
            [FromQuery] int? year = null)
        {
            try
            {
                // Nếu không truyền tháng/năm thì lấy tháng hiện tại
                month ??= DateTime.Now.Month;
                year ??= DateTime.Now.Year;

                var salary = await _userService.CalculateMonthSalaryAsync(tutorId, month.Value, year.Value);
                return Ok(new
                {
                    TutorId = tutorId,
                    Month = month,
                    Year = year,
                    Salary = salary
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/User/ProcessMonthEndSalary
        [HttpPost("ProcessMonthEndSalary")]
        public async Task<IActionResult> ProcessMonthEndSalary()
        {
            try
            {
                var result = await _userService.ProcessMonthEndSalaryAsync();
                return Ok(new { Message = "Salary processing completed", ProcessedCount = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
