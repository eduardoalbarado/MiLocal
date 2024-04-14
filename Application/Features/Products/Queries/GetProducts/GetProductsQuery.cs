using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : ProductDto, IRequest<PaginatedList<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? Enabled { get; set; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Product>();

        ProductPagedSpecifications spec = new(request.PageNumber, request.PageSize, request.Enabled);
        var products = await repository.ListAsync(spec, cancellationToken);
        var resultUOWDto = _mapper.Map<List<ProductDto>>(products);

        return new PaginatedList<ProductDto>(resultUOWDto, resultUOWDto.Count, request.PageNumber, request.PageSize);
    }
}
