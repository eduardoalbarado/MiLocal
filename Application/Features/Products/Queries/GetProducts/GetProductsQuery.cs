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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        // Get the repository for MyEntity
        var repository = _unitOfWork.GetRepository<Product>();

        ProductPagedSpecifications spec = new ProductPagedSpecifications(1, 10, true);
        var products = await repository.ListAsync();
        var resultUOWDto = _mapper.Map<List<ProductDto>>(products);

        return new PaginatedList<ProductDto>(resultUOWDto, resultUOWDto.Count, request.PageNumber, request.PageSize);
    }
}
