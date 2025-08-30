using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartWings.API.Middleware;
using SmartWings.Application.Contracts;
using SmartWings.Application.Interfaces;
using SmartWings.Application.Services;
using SmartWings.Domain.Interfaces;
using SmartWings.Infrastructure.Contracts;
using SmartWings.Infrastructure.DataContext;
using SmartWings.Infrastructure.Repositories;
using System.Text;


namespace SmartWings.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            // Configure database context
            builder.Services.AddDbContext<FlightDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("FlightDbConnection"),
                    b => b.MigrationsAssembly("SmartWings.Infrastructure")
                )
            );



            #region Configure Authentication Schema to validate Token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,

                };
            });
            #endregion
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            #region swagger token plug in generation code
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
            #endregion
            // Flight module
            builder.Services.AddScoped<IFlightService, FlightService>();
            builder.Services.AddScoped<IFlightRepository, FlightRepository>();

            // Booking module
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            // Admin Dashboard module
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            builder.Services.AddScoped<IAdminDashboardContract, AdminDashboardRepository>();

            // ? Payment module (UPI only)
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            // ? Notification module (Optional - if used)
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            // Authentication module
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            
            // OTP module for password reset
            builder.Services.AddScoped<IOtpRepository, OtpRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // User module
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // or "http://localhost:5173" for Vite
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // required if using cookies or Authorization header
                });
            });


            var app = builder.Build();

            //Global exception handler 
            app.UseGlobalExceptionHandler();
            app.UseCors("AllowAngularApp");

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
