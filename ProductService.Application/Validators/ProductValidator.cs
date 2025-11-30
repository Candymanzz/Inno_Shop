using FluentValidation;
using ProductService.Domain.Models;

namespace ProductService.Application.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200);

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.");

            RuleFor(p => p.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative.");
        }
    }
}
