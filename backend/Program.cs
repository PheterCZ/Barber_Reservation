using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Backend.Middlewares;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using backend.Interfaces;
using BarberOrder.backend.Configuration;

DotNetEnv.Env.Load();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var builder = WebApplication.CreateBuilder(args);

// Environment Variables
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
var emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
var emailUsername = Environment.GetEnvironmentVariable("EMAIL_USERNAME"); 

builder.Services.Configure<EmailSettings>(options =>
{
    var section = builder.Configuration.GetSection("EmailSettings");
    options.SmtpServer = section["SmtpServer"] ?? throw new InvalidOperationException("EmailSettings:SmtpServer is missing in configuration.");
    
    if (!int.TryParse(section["Port"], out var port))
    {
        throw new InvalidOperationException("EmailSettings:Port is missing or invalid.");
    }
    options.Port = port;
    
    options.Username = emailUsername ?? section["Username"] 
    ?? throw new InvalidOperationException("EmailSettings:Username is missing (check Environment Variables or appsettings).");

    options.Password = emailPassword ?? section["Password"] 
    ?? throw new InvalidOperationException("EmailSettings:Password is missing (check Environment Variables).");

    options.FromAddress = options.Username;
    options.EnableSsl = bool.TryParse(section["EnableSsl"], out var ssl) ? ssl : true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in configuration.");

builder.Configuration["SeedSettings:AdminEmail"] = adminEmail ?? throw new InvalidOperationException("Critical error: Environment variable ADMIN_EMAIL is missing!");

if(string.IsNullOrEmpty(dbPassword))
{
    throw new InvalidOperationException("Critical error: Environment variable DB_PASSWORD is missing!");
}
else 
{
    connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
}

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => 
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing!"))
        )
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IBarberService, BarberService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();  

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var config = services.GetRequiredService<IConfiguration>();
            var dbContext = services.GetRequiredService<ApplicationDBContext>();
            await dbContext.Database.MigrateAsync();
            await SeedData.Initialize(services, config);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogCritical(ex, "Critical error during application startup (Migrations/Seeding).");
        }
    }
}

app.Run();