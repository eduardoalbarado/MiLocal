using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : AddProductDto, IRequest<int>
    {
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.GetRepository<Product>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();

            // Check if the provided CategoryId exists
            var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (category == null)
            {
                var failure = new ValidationFailure(nameof(request.CategoryId), "Invalid CategoryId provided.");
                throw new ValidationException("Validation error occurred.", new List<ValidationFailure> { failure });
            }

            var product = _mapper.Map<Product>(request);
            await productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            // Create a new ProductCategory entity to establish the relationship
            var productCategory = new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = category.Id
            };

            // Add the ProductCategory to the database
            var productCategoryRepository = _unitOfWork.GetRepository<ProductCategory>();
            await productCategoryRepository.AddAsync(productCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }
    }
}
