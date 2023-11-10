using System;

using FluentValidation;
using TestProject.Domains;

namespace TestProject.FluentValidation
{
	public class ProductModelValidator: AbstractValidator<Product>
    {
        public ProductModelValidator()
        {
            RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Quantity is required.");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .Must(BeNumeric).WithMessage("Quantity must contain only numbers.");
            RuleFor(x => x.Price)
               .NotEmpty().WithMessage("Quantity is required.")
               .Must(BeNumeric1).WithMessage("Quantity must contain only numbers.");
        }
  private bool BeNumeric(int quantity)
    {
        // Use your custom logic to check if the quantity is numeric
        return int.TryParse(quantity.ToString(), out _);
    }
        private bool BeNumeric1(decimal quantity)
        {
            // Use your custom logic to check if the quantity is numeric
            return int.TryParse(quantity.ToString(), out _);
        }
    }
  
}

