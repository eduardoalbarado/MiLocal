using AutoMapper;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Get the repository for Product
            var repository = _unitOfWork.GetRepository<Product>();

            // Get the existing product from the repository
            var product = await repository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                return Result<ProductDto>.Failure($"Product with Id {request.Id} not found");
            }

            // Map the product to a DTO
            var productDto = _mapper.Map<ProductDto>(product);

            return Result<ProductDto>.Success(productDto);
        }
    }
}
