using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.ModelViews.AuthModelViews
{
    public class AddClaimToRoleModel
    {
        public Guid RoleId { get; set; }          // ID của vai trò (Role)
        public string ClaimType { get; set; }      // Loại claim (ví dụ: "Permission")
        public string ClaimValue { get; set; }     // Giá trị của claim (ví dụ: "Admin")
        public string CreatedBy { get; set; }      // Người tạo claim (ví dụ: tên người dùng hoặc ID)
    }

}
