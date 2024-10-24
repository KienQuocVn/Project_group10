using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnDemandTutor.Contract.Repositories.Entity;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Repositories.Context;
using OnDemandTutor.Repositories.Entity;
using OnDemandTutor.Services;
using OnDemandTutor.Services.Service;
using System.Configuration;
namespace OnDemandTutor.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddDatabase(configuration);
            services.AddAutoMapper();
            services.AddIdentity();
            services.AddInfrastructure(configuration);
            services.AddServices();
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
            {
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Accounts, ApplicationRole>(options =>
            {
            })
             .AddEntityFrameworkStores<DatabaseContext>()
             .AddDefaultTokenProviders();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services
                //.AddScoped<IUserService, UserService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IScheduleService, ScheduleService>()
                .AddScoped<IFeedbackService, FeedbackService>()
                .AddScoped<ISubjectService, SubjectService>()
                .AddScoped<ISlotSevice, SlotService>()
                 .AddScoped<IComplaintService, ComplaintService>()
                 .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IClassService, ClassService>()
                .AddScoped<IRequestRefundService, RequestRefundService>();
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

 

    }
}
