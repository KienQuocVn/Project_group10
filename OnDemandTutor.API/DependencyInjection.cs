using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Repositories.Context;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Repositories.UOW;
using OnDemandTutor.Services;
using OnDemandTutor.Services.Service;
using OnDemandTutor.Services.Service.AccountUltil;

namespace OnDemandTutor.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddDatabase(configuration);
            services.AddAutoMapper();
            services.AddConfigureServices();
            services.AddIdentityConfig();
            services.AddInfrastructure(configuration);
            services.AddServices();
            services.AddCorsConfig();
        }

        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void AddIdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<Accounts, ApplicationRole>()
                    .AddEntityFrameworkStores<DatabaseContext>()
                    .AddDefaultTokenProviders();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services

        .AddScoped<ITutorService, TutorService>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IScheduleService, ScheduleService>()
        .AddScoped<IFeedbackService, FeedbackService>()
        .AddScoped<ISubjectService, SubjectService>()
        .AddScoped<ISlotSevice, SlotService>()
        .AddScoped<IComplaintService, ComplaintService>()
        .AddScoped<IEmailSender, EmailSender>()
        .AddScoped<IClassService, ClassService>()
        .AddScoped<IRequestRefundService, RequestRefundService>()
        .AddScoped<AccountUtils>()
        .AddScoped<IUnitOfWork, UnitOfWork>()// Ensure AccountUtils is registered
        .AddScoped<IExportStudent, ExportStudent>()
        .AddScoped<TokenService>();
                
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }


        public static void AddConfigureServices(this IServiceCollection services)
        {
            services.AddRazorPages();
        }
    }
}
