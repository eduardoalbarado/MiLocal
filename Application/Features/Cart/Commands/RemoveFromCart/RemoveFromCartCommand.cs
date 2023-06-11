using Application.Common.Models.Responses;
using Application.Features.Carts.Queries.GetCartByUserId;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.RemoveFromCart;
public class RemoveFromCartCommand : IRequest<Result<Unit>>
{
    public int ItemId { get; set; }
}

public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public RemoveFromCartCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<Result<Unit>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);

        var repository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await repository.GetBySpecAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            return Result<Unit>.Failure($"Cart for user with UserId {userId} not found");
        }

        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == request.ItemId);

        if (cartItem == null)
        {
            return Result<Unit>.Failure($"CartItem with Id {request.ItemId} not found in the cart");
        }

        cart.Items.Remove(cartItem);

        await _unitOfWork.SaveChangesAsync();

        return Result<Unit>.Success(Unit.Value);
    }
}
