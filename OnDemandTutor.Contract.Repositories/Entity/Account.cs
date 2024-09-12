using OnDemandTutor.Core.Base;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class Account : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // học sinh, giáo viên, quản trị viên, kiểm duyệt viên
        public string? Otp { get; set; }

        // Navigation properties
        public virtual ICollection<Tutor> Tutors { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Moderator> Moderators { get; set; }
        public virtual ICollection<BanAccount> BanAccounts { get; set; }
    }
}
