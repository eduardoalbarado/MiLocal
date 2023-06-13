using FluentValidation;

namespace Application.Features.Products.Commands.RemoveCategoryFromProduct;

public class RemoveCategoryFromProductCommandValidator : AbstractValidator<RemoveCategoryFromProductCommand>
{
    public RemoveCategoryFromProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
    }
}
