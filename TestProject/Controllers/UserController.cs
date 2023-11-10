using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polly;
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
    private readonly IHttpClientFactory _httpClientFactory;
    public UserController(UserManager<User> userManager, AppDbContext context, IUserRepository userRepository, SignInManager<User> signInManager, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _signInManager = signInManager;
        _httpClientFactory = httpClientFactory;
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
        var checkUser = "";
        var retryPolicy = Policy.Handle<Exception>()
            .RetryAsync(3, (exception, retryCount) =>
            {
                Console.WriteLine($"An exception occurred during login. Retry attempt: {retryCount}. Exception: {exception}");
            });

        var signInResult = await retryPolicy.ExecuteAsync(async () =>
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            checkUser = await _userRepository.Login(model);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;
            }

            if (checkUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        });

        if (signInResult.Succeeded)
        {
            return signInResult.Succeeded ? RedirectToAction(checkUser is "ADMIN" ? "Index" : "UserView", "Products") : RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
   
}
