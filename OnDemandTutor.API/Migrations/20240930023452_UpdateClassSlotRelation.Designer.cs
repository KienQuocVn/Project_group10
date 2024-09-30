﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnDemandTutor.Repositories.Context;

#nullable disable

namespace OnDemandTutor.API.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240930023452_UpdateClassSlotRelation")]
    partial class UpdateClassSlotRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserRoles", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserTokens", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Class", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AmountOfSlot")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("EndDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("StartDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("TutorId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TutorId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Complaint", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SlotId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId", "TutorId");

                    b.HasIndex("SlotId");

                    b.HasIndex("TutorId");

                    b.ToTable("Complaints");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Feedback", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FeedbackText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("NumberOfViolations")
                        .HasColumnType("int");

                    b.Property<string>("SlotId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StudentId", "TutorId");

                    b.HasIndex("SlotId");

                    b.HasIndex("TutorId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("AccountsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bank")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("AccountsId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Schedule", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SlotId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SlotId");

                    b.HasIndex("StudentId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Slot", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClassId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DayOfSlot")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Slots");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Subject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.TutorSubject", b =>
                {
                    b.Property<Guid>("TutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("TutorId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("TutorSubjects");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.UserInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("Bank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LinkCV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Yob")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("OnDemandTutor.Repositories.Entity.Accounts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserInfoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserInfoId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationRoleClaims", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserClaims", b =>
                {
                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserLogins", b =>
                {
                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserRoles", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.ApplicationUserTokens", b =>
                {
                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Class", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.Subject", "Subject")
                        .WithMany("Classes")
                        .HasForeignKey("SubjectId");

                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", "Tutor")
                        .WithMany("Classes")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Complaint", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.Slot", "Slot")
                        .WithMany()
                        .HasForeignKey("SlotId");

                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", "Accounts")
                        .WithMany("Complaints")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounts");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Feedback", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.Slot", "Slot")
                        .WithMany("Feedbacks")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", "Accounts")
                        .WithMany("Feedbacks")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounts");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Payment", b =>
                {
                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", "Accounts")
                        .WithMany("Payments")
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Schedule", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.Slot", "Slot")
                        .WithMany("Schedules")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", "Student")
                        .WithMany("Schedules")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slot");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Slot", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.Class", "Class")
                        .WithMany("Slots")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.TutorSubject", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.Subject", "Subject")
                        .WithMany("TutorSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnDemandTutor.Repositories.Entity.Accounts", "Tutor")
                        .WithMany("TutorSubjects")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("OnDemandTutor.Repositories.Entity.Accounts", b =>
                {
                    b.HasOne("OnDemandTutor.Contract.Repositories.Entity.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoId");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Class", b =>
                {
                    b.Navigation("Slots");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Slot", b =>
                {
                    b.Navigation("Feedbacks");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("OnDemandTutor.Contract.Repositories.Entity.Subject", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("TutorSubjects");
                });

            modelBuilder.Entity("OnDemandTutor.Repositories.Entity.Accounts", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Complaints");

                    b.Navigation("Feedbacks");

                    b.Navigation("Payments");

                    b.Navigation("Schedules");

                    b.Navigation("TutorSubjects");
                });
#pragma warning restore 612, 618
        }
    }
}
