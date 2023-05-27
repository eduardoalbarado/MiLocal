using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommand : IRequest<Result<Unit>>
    {
        public Guid UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Cart>();
            var cartSpec = new CartByUserIdSpecification(request.UserId);
            var cart = await repository.GetBySpecAsync(cartSpec, cancellationToken);

            if (cart == null)
            {
                return Result<Unit>.Failure($"Cart for user with UserId {request.UserId} not found");
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == request.CartItemId);

            if (cartItem == null)
            {
                return Result<Unit>.Failure($"CartItem with Id {request.CartItemId} not found in the cart");
            }

            cartItem.Quantity = request.Quantity;

            await _unitOfWork.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
