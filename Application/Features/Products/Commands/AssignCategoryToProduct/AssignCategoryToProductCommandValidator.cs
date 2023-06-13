using FluentValidation;

namespace Application.Features.Products.Commands.AssignCategoryToProduct;

public class AssignCategoryToProductCommandValidator : AbstractValidator<AssignCategoryToProductCommand>
{
    public AssignCategoryToProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
    }
}
