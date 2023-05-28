using Application.Common.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Queries.GetCart;
public class GetCartByUserIdQuery : IRequest<Result<CartDto>>
{
    public GetCartByUserIdQuery()
    {
        UserId = Guid.Empty;
    }
    public Guid UserId { get; set; }
}

public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, Result<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetCartByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<Result<CartDto>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);

        var repository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await repository.GetBySpecAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            return Result<CartDto>.Failure($"Cart for user with UserId {userId} not found");
        }

        var cartDto = _mapper.Map<CartDto>(cart);

        return Result<CartDto>.Success(cartDto);
    }
}
