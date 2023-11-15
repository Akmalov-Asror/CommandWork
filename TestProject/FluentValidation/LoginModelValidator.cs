using FluentValidation;
using TestProject.ExtensionFunctions;
using TestProject.ViewModels;

namespace TestProject.FluentValidation;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Must(CheckEmail.HaveCapitalLetter).WithMessage("Password must contain at least one capital letter.");
    }
}