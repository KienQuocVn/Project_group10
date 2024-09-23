using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.AuthModelViews
{
    public class AddRoleModel
    {
        [Required]
        public Guid UserId { get; set; }  // Kiểu dữ liệu GUID để nhận đúng từ JSON

        [Required]
        public string RoleName { get; set; }  // Vai trò của người dùng
    }

}
