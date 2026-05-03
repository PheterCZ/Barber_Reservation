
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.Interfaces;
using backend.Services;
using BarberOrder.backend.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace backend.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarberOrder API", Version = "v1", Description = "Backend rozhraní pro správu rezervací v holičství." });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Zadejte JWT token takto: Bearer {váš_token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

        }
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => 
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing!"))
                    )
                };
            });
        }
 
        public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(options =>
            {
                var section = configuration.GetSection("EmailSettings");
                options.SmtpServer = section["SmtpServer"] ?? throw new InvalidOperationException("EmailSettings:SmtpServer is missing in configuration.");
                
                if (!int.TryParse(section["Port"], out var port))
                {
                    throw new InvalidOperationException("EmailSettings:Port is missing or invalid.");
                }
                options.Port = port;
                
                options.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? section["Username"] 
                ?? throw new InvalidOperationException("EmailSettings:Username is missing (check Environment Variables or appsettings).");

                options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? section["Password"] 
                ?? throw new InvalidOperationException("EmailSettings:Password is missing (check Environment Variables).");

                options.FromAddress = options.Username;
                options.EnableSsl = bool.TryParse(section["EnableSsl"], out var ssl) ? ssl : true;
            });
        }

        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in configuration.");

            configuration["SeedSettings:AdminEmail"] = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? throw new InvalidOperationException("Critical error: Environment variable ADMIN_EMAIL is missing!");

            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_PASSWORD")))
            {
                throw new InvalidOperationException("Critical error: Environment variable DB_PASSWORD is missing!");
            }
            else 
            {
                connectionString = connectionString.Replace("{DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));
            }


            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IBarberService, BarberService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
        }

    }
}