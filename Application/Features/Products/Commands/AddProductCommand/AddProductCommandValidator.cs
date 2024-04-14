using FluentValidation;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.ShortName)
                .NotEmpty().WithMessage("ShortName is required.")
                .MaximumLength(50).WithMessage("ShortName must not exceed 50 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(p => p.PriceInDollars)
                .GreaterThanOrEqualTo(0).WithMessage("PriceInDollars must be greater than or equal to 0.");

            RuleFor(p => p.Cost)
                .GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or equal to 0.");

            RuleFor(p => p.CostInDollars)
                .GreaterThanOrEqualTo(0).WithMessage("CostInDollars must be greater than or equal to 0.");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.")
                .Must((command, categoryId) => categoryId != null).WithMessage("CategoryId must not be null.");
        }
    }
}
