using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Repositories.Context
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRoles, ApplicationUserLogins, ApplicationRoleClaims, ApplicationUserTokens>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // user
        public virtual DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public virtual DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public virtual DbSet<ApplicationUserClaims> ApplicationUserClaims => Set<ApplicationUserClaims>();
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles => Set<ApplicationUserRoles>();
        public virtual DbSet<ApplicationUserLogins> ApplicationUserLogins => Set<ApplicationUserLogins>();
        public virtual DbSet<ApplicationRoleClaims> ApplicationRoleClaims => Set<ApplicationRoleClaims>();
        public virtual DbSet<ApplicationUserTokens> ApplicationUserTokens => Set<ApplicationUserTokens>();

        public virtual DbSet<UserInfo> UserInfos => Set<UserInfo>();
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<BanAccount> BanAccounts { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<CV> CVs { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Moderator> Moderators { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }
        public virtual DbSet<TutorSubject> TutorSubjects { get; set; }
        //public virtual DbSet<Wallet> Wallets { get; set; }
        //public virtual DbSet<Payment> Payments { get; set; }
        //public virtual DbSet<ReasonDenyCv> ReasonDenyCvs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example of a many-to-many relationship between Tutor and Subject via TutorSubject
            modelBuilder.Entity<TutorSubject>()
                .HasKey(ts => new { ts.TutorId, ts.SubjectId });

            modelBuilder.Entity<TutorSubject>()
                .HasOne(ts => ts.Tutor)
                .WithMany(t => t.TutorSubjects)
                .HasForeignKey(ts => ts.TutorId);

            modelBuilder.Entity<TutorSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TutorSubjects)
                .HasForeignKey(ts => ts.SubjectId);

            // Configure other relationships here as needed
        }
    }
}
