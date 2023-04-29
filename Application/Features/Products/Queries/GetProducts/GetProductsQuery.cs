using Application.Common.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : ProductDto, IRequest<PaginatedList<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool Enabled { get; set; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IRepositoryAsync<Product> _repositoryAsync;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IMapper mapper, IRepositoryAsync<Product> repositoryAsync)
    {
        _mapper = mapper;
        _repositoryAsync = repositoryAsync;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductPagedSpecifications(request.PageNumber, request.PageSize, request.Enabled);
        var products = await _repositoryAsync.ListAsync(spec, cancellationToken);
        var productsCount = await _repositoryAsync.CountAsync(spec, cancellationToken);
        var result = _mapper.Map<List<ProductDto>>(products);

        return new PaginatedList<ProductDto>(result, productsCount, request.PageNumber, request.PageSize);
    }
}
