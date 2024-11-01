using Application.Common.Models;
using Application.Common.Models.Responses;
using Application.Exceptions;
using Application.Features.Products.Queries.GetProductById;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Features.Products.Commands.AddCategoryToProduct;

public class AddCategoryToProductCommand : IRequest<Result<ProductDto>>
{
    public int ProductId { get; set; }
    public CategoryDto Category { get; set; }
}

public class AddCategoryToProductCommandHandler : IRequestHandler<AddCategoryToProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddCategoryToProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ProductDto>> Handle(AddCategoryToProductCommand request, CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.GetRepository<Product>();
        var categoryRepository = _unitOfWork.GetRepository<Category>();
        var productCategoryRepository = _unitOfWork.GetRepository<ProductCategory>();

        // Check if the product exists
        var spec = new ProductByIdSpecifications(request.ProductId);
        var product = await productRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        // Check if the category exists
        var category = await categoryRepository.GetByIdAsync(request.Category.Id, cancellationToken);
        if (category == null)
        {
            var failure = new ValidationFailure(nameof(request.Category.Id), $"Category with ID {request.Category.Id} not found.");
            throw new ValidationException("Validation error occurred.", new List<ValidationFailure> { failure });
        }

        // Check if the product already has the category assigned
        if (product.ProductCategories?.Any(pc => pc.CategoryId == category.Id) ?? false)
        {
            throw new BadRequestException($"Product already has category '{category.Name}' assigned.");
        }

        // Create a new ProductCategory entity to establish the relationship
        var productCategory = new ProductCategory
        {
            ProductId = product.Id,
            CategoryId = category.Id
        };

        await productCategoryRepository.AddAsync(productCategory, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        product = await productRepository.FirstOrDefaultAsync(spec, cancellationToken);
        var productDto = _mapper.Map<ProductDto>(product);

        return Result<ProductDto>.Success(productDto);
    }
}
