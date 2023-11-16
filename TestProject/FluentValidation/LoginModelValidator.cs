using FluentValidation;
using TestProject.ExtensionFunctions;
using TestProject.ViewModels;

namespace TestProject.FluentValidation;
/// <summary>
/// Validator class used for validating the properties of a LoginModel.
/// </summary>
/// <remarks>
///     The LoginModelValidator class inherits from AbstractValidator<T>,
///     which is part of the FluentValidation library used for defining validation rules.
///     It validates the Email and Password properties of a LoginModel.
/// </remarks>
public class LoginModelValidator : AbstractValidator<LoginModel>
{
    /// <summary>
    /// Initializes a new instance of the LoginModelValidator class.
    /// </summary>
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