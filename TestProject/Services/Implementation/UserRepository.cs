using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Domains;
using TestProject.Services.Interfaces;
using TestProject.ViewModels;

namespace TestProject.Services.Implementation;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<RegisterModel> Register(RegisterModel model)
    {
        var user = new User { UserName = model.Name, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "USER");
            await _context.SaveChangesAsync();
        }
        foreach (var error in result.Errors)
        {
            throw new Exception($"{error.Description}");
        }

        return model ?? new RegisterModel();

    }

    public async Task<RegisterModel> RegisterAdmin(RegisterModel model)
    {
        var user = new User
        {
            UserName = model.Name,
            Email = model.Email,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        Console.WriteLine(result.Errors);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "ADMIN");
            await _context.SaveChangesAsync();
        }
        foreach (var error in result.Errors)
        {
            throw new Exception($"{error.Description}");
        }
        return model ?? new RegisterModel();
    }

    public async Task<string> Login(LoginModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles[0];
        if (user is null)
        {
            return new string(roles[0]);
        }

        return role;
    }
}