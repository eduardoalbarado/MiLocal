using Application.Exceptions;
using Application.Features.Categories.Queries.GetCategories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.RemoveCategoryFromProduct;

public class RemoveCategoryFromProductCommand : IRequest<Unit>
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
}

public class RemoveCategoryFromProductCommandHandler : IRequestHandler<RemoveCategoryFromProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveCategoryFromProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RemoveCategoryFromProductCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Product>();
        var product = await repository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException("Product", request.ProductId);
        }

        var categoryRepository = _unitOfWork.GetRepository<Category>();
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException("Category", request.CategoryId);
        }

        var productCategoryRepository = _unitOfWork.GetRepository<ProductCategory>();
        var spec = new ProductCategorySpecifications(request.ProductId, request.CategoryId);
        var productCategoryToDelete = await productCategoryRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (productCategoryToDelete is not null)
        {
            await productCategoryRepository.DeleteAsync(productCategoryToDelete, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        return Unit.Value;
    }
}
