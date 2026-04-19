using backend.Data;
using backend.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nebyl nalezen v konfiguraci.");

var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");


Console.WriteLine("========================================");
Console.WriteLine($"DEBUG: Moje heslo v C# je: [{dbPassword}]");
Console.WriteLine("========================================");

if(string.IsNullOrEmpty(dbPassword))
{
    throw new InvalidOperationException("Kritická chyba: Chybí proměnná prostředí DB_PASSWORD!");
}
else{
    connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
}

// NAHORU pod Replace:
Console.WriteLine($"--- KONTROLA: '{connectionString}' ---");


builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddExceptionHandler<ExceptionMiddleware>();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    dbContext.Database.Migrate();
}


app.Run();
