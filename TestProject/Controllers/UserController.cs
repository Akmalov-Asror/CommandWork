using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Domains;
using TestProject.ViewModels;

namespace TestProject.Controllers;

public class UserController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER");
                await _context.SaveChangesAsync();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        }
        return RedirectToAction("Index", "Home");
    }
    public IActionResult RegisterAdmin() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAdmin(RegisterModel model)
    {
        if (ModelState.IsValid)
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
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }
}
