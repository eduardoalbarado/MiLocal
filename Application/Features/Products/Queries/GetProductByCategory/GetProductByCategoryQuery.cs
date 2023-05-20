using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProductByCategory
{
    public class GetProductsByCategoryQuery : IRequest<PaginatedList<ProductDto>>
    {
        public int Category { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? Enabled { get; set; }
    }

    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, PaginatedList<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByCategoryQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            // Get the repository for Product
            var repository = _unitOfWork.GetRepository<Product>();

            ProductPagedSpecifications spec = new(request.PageNumber, request.PageSize, request.Enabled, request.Category);
            var products = await repository.ListAsync(spec, cancellationToken);
            var resultDto = _mapper.Map<List<ProductDto>>(products);

            return new PaginatedList<ProductDto>(resultDto, resultDto.Count, request.PageNumber, request.PageSize);
        }
    }
}
