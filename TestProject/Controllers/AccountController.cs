﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Polly;
using TestProject.Domains;
using TestProject.FluentValidation;
using TestProject.Services.Interfaces;
using TestProject.ViewModels;

namespace TestProject.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IToastNotification _toastNotification;
    private readonly SignInManager<User> _signInManager;

    public AccountController(IToastNotification toastNotification, IUserRepository userRepository, SignInManager<User> signInManager)
    { 
        _toastNotification = toastNotification;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
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
        try
        {
            if (ModelState.IsValid) await _userRepository.Register(model);
            _toastNotification.AddSuccessToastMessage("Registration successfully!");
            return RedirectToAction("Index", "Home");
        }
        catch(Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(model);
        }
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
        try
        {
            await _userRepository.RegisterAdmin(model);
         
            _toastNotification.AddSuccessToastMessage("Registration successfully!");

            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(model);
        }
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
     
        var retryPolicy = Policy.Handle<Exception>()
            .RetryAsync(3, (exception, retryCount) =>
            {
                Console.WriteLine($"An exception occurred during login. Retry attempt: {retryCount}. Exception: {exception}");
            });

        try
        {
            var signInResult = await retryPolicy.ExecuteAsync(async () =>
            {
                var result = await _userRepository.Login(model);
                return result;
            });


            return RedirectToAction("Index", "Products");
        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(model);
        }
        
    }
    public IActionResult AccessDenied() => RedirectToAction("Index", "Products");

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Main", "Account");
    }
    public async Task<IActionResult> Main()
    {
        return User.Identity!.IsAuthenticated ? RedirectToAction("Index", "Products") : View();
    }
}
