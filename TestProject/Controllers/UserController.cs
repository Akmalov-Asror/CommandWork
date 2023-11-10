using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
using TestProject.Data;
using TestProject.Domains;
using TestProject.FluentValidation;
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
        var validationResult = await new RegisterModelValidator().ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View(model);
        }

        await _userRepository.RegisterAdmin(model);
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Login() => View();


    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var validationResult = await new LoginModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View(model);
        }
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
