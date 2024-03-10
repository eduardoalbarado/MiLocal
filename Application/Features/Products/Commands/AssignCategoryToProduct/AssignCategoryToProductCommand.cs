using Application.Exceptions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.AssignCategoryToProduct;

public class AssignCategoryToProductCommand : IRequest<Unit>
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
}

public class AssignCategoryToProductCommandHandler : IRequestHandler<AssignCategoryToProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignCategoryToProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignCategoryToProductCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Product>();
        var product = await repository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new NotFoundException("Product", request.ProductId);
        }

        var categoryRepository = _unitOfWork.GetRepository<Category>();
        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new NotFoundException("Category", request.CategoryId);
        }
        var productCategoryRepository = _unitOfWork.GetRepository<ProductCategory>();
        var productCategory = new ProductCategory
        {
            ProductId = request.ProductId,
            CategoryId = request.CategoryId
        };
        await productCategoryRepository.AddAsync(productCategory, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
