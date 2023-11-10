using Microsoft.AspNetCore.Identity;
using TestProject.ViewModels;

namespace TestProject.Services.Interfaces;

public interface IUserRepository
{
    Task<RegisterModel> Register(RegisterModel model);
    Task<RegisterModel> RegisterAdmin(RegisterModel model);
    Task<SignInResult> Login(LoginModel model);
}