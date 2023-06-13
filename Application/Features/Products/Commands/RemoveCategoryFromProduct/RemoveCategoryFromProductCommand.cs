using Application.Exceptions;
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

        product.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
