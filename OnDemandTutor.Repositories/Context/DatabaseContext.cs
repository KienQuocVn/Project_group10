using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Repositories.Entity;

namespace OnDemandTutor.Repositories.Context
{
    public class DatabaseContext : IdentityDbContext<Accounts, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRoles, ApplicationUserLogins, ApplicationRoleClaims, ApplicationUserTokens>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // user
        public virtual DbSet<Accounts> ApplicationUsers => Set<Accounts>();
        public virtual DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public virtual DbSet<ApplicationUserClaims> ApplicationUserClaims => Set<ApplicationUserClaims>();
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles => Set<ApplicationUserRoles>();
        public virtual DbSet<ApplicationUserLogins> ApplicationUserLogins => Set<ApplicationUserLogins>();
        public virtual DbSet<ApplicationRoleClaims> ApplicationRoleClaims => Set<ApplicationRoleClaims>();
        public virtual DbSet<ApplicationUserTokens> ApplicationUserTokens => Set<ApplicationUserTokens>();

        public virtual DbSet<UserInfo> UserInfos => Set<UserInfo>();
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<TutorSubject> TutorSubjects { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TutorSubject>()
                .HasKey(ts => new { ts.TutorId, ts.SubjectId });

            modelBuilder.Entity<TutorSubject>()
                .HasOne(ts => ts.Tutor)
                .WithMany(t => t.TutorSubjects)
                .HasForeignKey(ts => ts.TutorId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TutorSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TutorSubjects)
                .HasForeignKey(ts => ts.SubjectId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Complaint>()
                .HasKey(ts => new { ts.StudentId, ts.TutorId});

            modelBuilder.Entity<Complaint>()
                .HasOne(ts => ts.Accounts)
                .WithMany(t => t.Complaints)
                .HasForeignKey(ts => ts.StudentId);

            modelBuilder.Entity<Complaint>()
                .HasOne(ts => ts.Accounts)
                .WithMany(s => s.Complaints)
                .HasForeignKey(ts => ts.TutorId);

            modelBuilder.Entity<Feedback>()
               .HasOne(ts => ts.Slot)
               .WithMany(s => s.Feedbacks)
               .HasForeignKey(ts => ts.SlotId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasKey(ts => new { ts.StudentId, ts.TutorId });

            modelBuilder.Entity<Feedback>()
                .HasOne(ts => ts.Accounts)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(ts => ts.StudentId);

            modelBuilder.Entity<Feedback>()
                .HasOne(ts => ts.Accounts)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(ts => ts.TutorId);

            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Slots)
                .HasForeignKey(s => s.ClassId);
        }
    }
}
