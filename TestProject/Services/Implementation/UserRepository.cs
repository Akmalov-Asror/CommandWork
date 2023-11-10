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
    private readonly SignInManager<User> _signInManager;

    public UserRepository(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<RegisterModel> Register(RegisterModel model)
    {
        var existUser = await _userManager.FindByEmailAsync(model.Email);
        if (existUser != null)
        {
            throw new Exception("Email already taken ");
        }
        var user = new User { UserName = model.Name, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "USER");
            await _context.SaveChangesAsync();
        }
        foreach (var error in result.Errors)
            throw new Exception($"{error.Description}");

        return model ?? new RegisterModel();

    }

    public async Task<RegisterModel> RegisterAdmin(RegisterModel model)
    {
        var existUser = await _userManager.FindByEmailAsync(model.Email);
        if (existUser != null)
        {
            throw new Exception("Email already taken ");   
        }
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
        foreach (var error in result.Errors) throw new Exception($"{error.Description}");

        return model ?? new RegisterModel();
    }

    public async Task<SignInResult> Login(LoginModel model)
    {

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            throw new Exception("Invalid email or password");
        }
        var passResult = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!passResult)
        {
            throw new Exception("Invalid email or password");
        }
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        if (!result.Succeeded)
        {
            throw new Exception("Invalid email or password");
        }
        return result;
    }
}