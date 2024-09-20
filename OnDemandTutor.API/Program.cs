using Microsoft.EntityFrameworkCore;
using OnDemandTutor.API;
using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Repositories.Context;
using OnDemandTutor.Repositories.UOW;
using OnDemandTutor.Services.Service;

var builder = WebApplication.CreateBuilder(args);

// C?u h?nh DbContext v?i SQL Server  
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("OnDemandTutor.API"));
});

// Th?m c?c d?ch v? v?o container  
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<ITutorService, TutorService>();

// C?u h?nh appsettings theo m?i tr??ng  
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// C?u h?nh CORS  
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Th?m c?c d?ch v? ?i?u khi?n  
builder.Services.AddControllers();

// C?u h?nh Swagger/OpenAPI  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Th?m c?u h?nh t?y ch?nh n?u c?n  
builder.Services.AddConfig(builder.Configuration);

var app = builder.Build();

// C?u h?nh pipeline HTTP request  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();