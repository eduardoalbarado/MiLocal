using Application.Common.Models.Responses;
using Application.Features.Carts.Queries.GetCartByUserId;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommand : UpdateCartItemQuantityDto, IRequest<Result<Unit>>
{
    public int CartItemId { get; set; }
}

public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<Result<Unit>> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);

        var repository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await repository.GetBySpecAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            return Result<Unit>.Failure($"Cart for user with UserId {userId} not found");
        }

        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == request.CartItemId);

        if (cartItem == null)
        {
            return Result<Unit>.Failure($"CartItem with Id {request.CartItemId} not found in the cart");
        }

        cartItem.Quantity = request.Quantity;
        cart.LastModified = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return Result<Unit>.Success(Unit.Value);
    }
}
