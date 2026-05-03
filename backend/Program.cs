using backend.Extensions;
using Backend.Middlewares;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

DotNetEnv.Env.Load();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Volání tvých nových extension metod
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddEmailConfiguration(builder.Configuration);
builder.Services.AddApplicationServices(); 
builder.Services.AddCustomCors(); 

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();  

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

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
            logger.LogCritical(ex, "Kritická chyba při startu (Migrace/Seed).");
        }
    }
}

app.UseHttpsRedirection();
app.UseCors(); 

app.UseExceptionHandler(); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();