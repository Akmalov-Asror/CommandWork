using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Domains;
using TestProject.Services.Interfaces;
using TestProject.ViewModels;

namespace TestProject.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserController(UserManager<User> userManager, AppDbContext context, IUserRepository userRepository, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid) await _userRepository.Register(model);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult RegisterAdmin() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAdmin(RegisterModel model)
    {
        if (ModelState.IsValid) await _userRepository.RegisterAdmin(model);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        // find user Role !
        var checkUser =  await _userRepository.Login(model);
        if (user == null) ModelState.AddModelError(string.Empty, "Invalid email or password");
        if (checkUser == null) ModelState.AddModelError(string.Empty, "Invalid email or password");
        // next step I added user Role !
        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        // finally redirection 
        return signInResult.Succeeded ? RedirectToAction(checkUser is "ADMIN" ? "Index" : "UserView", "Products") : RedirectToAction("Index", "Home");

    }
}
