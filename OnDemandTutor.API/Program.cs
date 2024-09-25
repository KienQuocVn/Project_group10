using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.API;

using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Repositories.Context;

using OnDemandTutor.Contract.Repositories.Interface;
using OnDemandTutor.Contract.Repositories.IUOW;
using OnDemandTutor.Contract.Services.Interface;
using OnDemandTutor.Repositories.Context;


using OnDemandTutor.Repositories.UOW;

using OnDemandTutor.Services.Service;

var builder = WebApplication.CreateBuilder(args);


// Cấu hình DbContext

// C?u h?nh DbContext v?i SQL Server  

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("OnDemandTutor.API"));
});

// Add services to the container.
builder.Services.AddScoped<IVNPayService, VnPayService>(); 
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<ITutorService, TutorService>();
builder.Services.AddScoped<IVNPayService, VnPayService>(); 



// Cấu hình appsettings theo môi trường


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<ITutorService, TutorService>();

// C?u h?nh appsettings theo m?i tr??ng  

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


//add to swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Đăng ký TokenService
builder.Services.AddScoped<TokenService>();

// Thêm các dịch vụ khác
builder.Services.AddControllers();

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


// Cấu hình pipeline HTTP

// C?u h?nh pipeline HTTP request  

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();  // Thêm middleware cho JWT Authentication


app.UseAuthorization();
app.MapControllers();
app.Run();