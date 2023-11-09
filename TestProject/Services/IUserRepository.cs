using TestProject.ViewModels;

namespace TestProject.Services;

public interface IUserRepository
{
    Task<RegisterModel> Register(RegisterModel model);
    Task<RegisterModel> RegisterAdmin(RegisterModel model);
    Task<string> Login(LoginModel model);

}