using OnDemandTutor.Core.Base;

namespace OnDemandTutor.Contract.Repositories.Entity
{
    public class UserInfo : BaseEntity
    {
        //public string UserName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LinkCV{ get; set; }
        public DateTime? Yob {get; set; }
        //public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public double Balance { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public string? Bank { get; set; }
    }
}
