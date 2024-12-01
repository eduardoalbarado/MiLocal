using Application.Common.Models.Responses;
using Application.Features.Carts.Queries.GetCart;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Queries.GetCarts;
public class GetCartsQuery : IRequest<Result<List<CartDto>>>
{
    public GetCartsQuery()
    {
        UserId = Guid.Empty;
    }
    public Guid UserId { get; set; }
}

public class GetCartsQueryHandler : IRequestHandler<GetCartsQuery, Result<List<CartDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetCartsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<Result<List<CartDto>>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);

        var repository = _unitOfWork.GetRepository<Cart>();
        var carts = await repository.ListAsync(cancellationToken);

        var cartDto = _mapper.Map<List<CartDto>>(carts);

        return Result<List<CartDto>>.Success(cartDto);
    }
}
