using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Domain.Enums;
using SportRace.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SportRaceDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTP се използва локално за проекта.
// app.UseHttpsRedirection();

app.UseAuthorization();

// Създаване на първоначален администратор,
// ако такъв все още не съществува.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SportRaceDbContext>();

    const string adminEmail = "admin@sportrace.local";

    var adminExists = db.Users.Any(u => u.Email == adminEmail);

    if (!adminExists)
    {
        var admin = new User
        {
            FirstName = "SportRace",
            LastName = "Admin",
            Email = adminEmail,
            Role = UserRole.Administrator,
            CreatedAt = DateTime.UtcNow
        };

        admin.PasswordHash =
            new PasswordHasher<User>().HashPassword(admin, "Admin123!");

        db.Users.Add(admin);
        db.SaveChanges();
    }
}

app.MapControllers();

app.Run();