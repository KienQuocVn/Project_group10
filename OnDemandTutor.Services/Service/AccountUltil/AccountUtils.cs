using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Services.Service.AccountUltil
{
    public class AccountUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationRepository _userRepository;

        public AccountUtils(IHttpContextAccessor httpContextAccessor, IAuthenticationRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public Accounts GetCurrentUser()
        {
            // Lấy email của người dùng hiện tại từ Claims
            var email = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
            {
                return null;
            }

            // Tìm người dùng trong cơ sở dữ liệu bằng email
            var user = _userRepository.FindByEmail(email);
            return user;
        }
    }
}
