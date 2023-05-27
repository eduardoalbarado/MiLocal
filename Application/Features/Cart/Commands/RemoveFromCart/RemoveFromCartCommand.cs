using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest<Result<Unit>>
    {
        public Guid UserId { get; set; }
        public int CartItemId { get; set; }
    }

    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
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

            cart.Items.Remove(cartItem);

            await _unitOfWork.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
