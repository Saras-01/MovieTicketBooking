using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Constants;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data;

public static class RoleSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher<User>>();

        var accounts = new[]
        {
            new
            {
                FullName = "System Admin",
                Email = "admin@moviebooking.com",
                PhoneNumber = "9999999999",
                Role = UserRoles.Admin,
                Password = configuration["SeedUsers:AdminPassword"]
            },
            new
            {
                FullName = "Second Admin",
                Email = "admin2@moviebooking.com",
                PhoneNumber = "9999999997",
                Role = UserRoles.Admin,
                Password = configuration["SeedUsers:Admin2Password"]
            }
        };

        foreach (var account in accounts)
        {
            if (string.IsNullOrWhiteSpace(account.Password))
            {
                throw new InvalidOperationException(
                    $"Password configuration missing for {account.Email}");
            }

            var existingUser = await context.Users
                .FirstOrDefaultAsync(
                    x => x.Email == account.Email);

            if (existingUser != null)
            {
                continue;
            }

            var user = new User
            {
                FullName = account.FullName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Role = account.Role,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = passwordHasher.HashPassword(
                user,
                account.Password);

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();

        Console.WriteLine("Role seeding completed successfully.");
    }
}