using Microsoft.AspNetCore.Identity;

namespace BloodDonation.Web.Identity;

public static class IdentitySeeder
{
    // Creates the three roles, and in Development one test user per role from the "SeedUsers" config section.
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, IHostEnvironment environment)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!environment.IsDevelopment())
        {
            return;
        }

        foreach (var role in Roles.All)
        {
            var email = configuration[$"SeedUsers:{role}:Email"];
            var password = configuration[$"SeedUsers:{role}:Password"];
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                continue;
            }

            if (await userManager.FindByEmailAsync(email) is not null)
            {
                continue;
            }

            var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not seed {role} user: {string.Join("; ", result.Errors.Select(e => e.Description))}");
            }

            await userManager.AddToRoleAsync(user, role);
        }
    }
}
