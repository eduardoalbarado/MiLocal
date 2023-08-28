using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetCategoryProducts;

public class GetCategoryProductsQuery : IRequest<PaginatedList<ProductDto>>
{
    public int CategoryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? Enabled { get; set; }
}

public class GetCategoryProductsQueryHandler : IRequestHandler<GetCategoryProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryProductsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetCategoryProductsQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Product>();

        ProductPagedSpecifications spec = new(request.PageNumber, request.PageSize, request.Enabled, request.CategoryId);
        var products = await repository.ListAsync(spec, cancellationToken);
        var resultDto = _mapper.Map<List<ProductDto>>(products);

        return new PaginatedList<ProductDto>(resultDto, resultDto.Count, request.PageNumber, request.PageSize);
    }
}
