using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Repositories.Entity;
using System;

namespace OnDemandTutor.Repositories.Context
{
    public class DatabaseContext : IdentityDbContext<Accounts, ApplicationRole, Guid, ApplicationUserClaims, ApplicationUserRoles, ApplicationUserLogins, ApplicationRoleClaims, ApplicationUserTokens>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // User entities
        public virtual DbSet<Accounts> ApplicationUsers => Set<Accounts>();
        public virtual DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public virtual DbSet<ApplicationUserClaims> ApplicationUserClaims => Set<ApplicationUserClaims>();
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles => Set<ApplicationUserRoles>();
        public virtual DbSet<ApplicationUserLogins> ApplicationUserLogins => Set<ApplicationUserLogins>();
        public virtual DbSet<ApplicationRoleClaims> ApplicationRoleClaims => Set<ApplicationRoleClaims>();
        public virtual DbSet<ApplicationUserTokens> ApplicationUserTokens => Set<ApplicationUserTokens>();

        // Custom entities
        public DbSet<Accounts> Students { get; set; }
        public virtual DbSet<UserInfo> UserInfos => Set<UserInfo>();
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<TutorSubject> TutorSubjects { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; } // Thêm bảng Booking

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TutorSubject relationships
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

            // Schedule relationships
            modelBuilder.Entity<Schedule>()
                .HasKey(ts => new { ts.StudentId, ts.SlotId });

            modelBuilder.Entity<Schedule>()
                .HasOne(ts => ts.Student)
                .WithMany(t => t.Schedules)
                .HasForeignKey(ts => ts.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(ts => ts.Slot)
                .WithMany(s => s.Schedules)
                .HasForeignKey(ts => ts.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint relationships
            modelBuilder.Entity<Complaint>()
                .HasKey(ts => new { ts.StudentId, ts.TutorId });

            modelBuilder.Entity<Complaint>()
                .HasOne(ts => ts.Accounts)
                .WithMany(t => t.Complaints)
                .HasForeignKey(ts => ts.StudentId);

            modelBuilder.Entity<Complaint>()
                .HasOne(ts => ts.Accounts)
                .WithMany(s => s.Complaints)
                .HasForeignKey(ts => ts.TutorId);

            // Feedback relationships
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

            // Booking relationships
            modelBuilder.Entity<Booking>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Student)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Subject)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.TutorSubject)
                .WithMany(ts => ts.Bookings)
                .HasForeignKey(b => b.TutorSubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
