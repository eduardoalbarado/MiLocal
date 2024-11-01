using Application.Common.Models;
using FluentValidation;

namespace Application.Features.Products.Commands.AddCategoryToProduct;

public class AddCategoryToProductCommandValidator : AbstractValidator<AddCategoryToProductCommand>
{
    public AddCategoryToProductCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0.");

        RuleFor(x => x.Category)
            .NotNull().WithMessage("{PropertyName} cannot be null.")
            .SetValidator(new CategoryDtoValidator());
    }
}

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
    }
}