using Microsoft.AspNetCore.Identity;
using TestProject.Domains;
using TestProject.Domains.Enums;

namespace TestProject.Data;

public class Seed
{
    public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
    {
        using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            try
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await SeedRolesAsync(roleManager);

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during seeding: {ex.Message}");
            }
        }
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        await CreateRoleAsync(roleManager, ERole.ADMIN);
        await CreateRoleAsync(roleManager, ERole.USER);
    }

    private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, ERole role)
    {
        var roleName = role.ToString();

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
            Console.WriteLine($"{roleName} role created successfully.");
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        await CreateUserAsync(userManager, "Jenny@gmail.com", "Jenny", "A0601221a_", ERole.ADMIN);
        await CreateUserAsync(userManager, "Vin@gmail.com", "Vin", "B0601221b_", ERole.USER);
    }

    private static async Task CreateUserAsync(UserManager<User> userManager, string email, string userName, string password, ERole role)
    {
        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser == null)
        {
            var newUser = new User
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, role.ToString());
                Console.WriteLine($"{userName} user created and added to {role} role successfully.");
            }
            else Console.WriteLine($"Error creating {userName} user: {string.Join(", ", result.Errors)}");
        }
        else Console.WriteLine($"{userName} user already exists.");
    }
}