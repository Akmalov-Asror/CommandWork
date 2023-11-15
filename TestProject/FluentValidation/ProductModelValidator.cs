using System;
using FluentValidation;
using TestProject.Domains;

namespace TestProject.FluentValidation;

public class ProductModelValidator : AbstractValidator<Product>
{
    public ProductModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Quantity is required.");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required.")
            .Must(x => x >= 0).WithMessage("Quantity must be a non-negative number.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .Must(x => x >= 0).WithMessage("Price must be a non-negative number.");
    }
}