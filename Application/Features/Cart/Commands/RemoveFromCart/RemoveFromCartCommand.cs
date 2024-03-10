using Application.Common.Models.Responses;
using Application.Exceptions;
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
        try
        {
            var userId = Guid.Parse(_userContextService.GetUserContext().UserId);
            var cart = await GetCartForUser(userId, cancellationToken);
            var cartItem = GetCartItemById(cart, request.ItemId);

            var product = await GetProductForCartItem(cartItem, cancellationToken);

            UpdateProductStock(product, cartItem.Quantity);
            RemoveCartItemFromCart(cart, cartItem);

            await _unitOfWork.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            // TODO Log a failure.
            return Result<Unit>.Failure($"An error occurred: {ex.Message}");
        }
    }

    private async Task<Cart> GetCartForUser(Guid userId, CancellationToken cancellationToken)
    {
        var cartRepository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        return await cartRepository.GetBySpecAsync(cartSpec, cancellationToken);
    }

    private CartItem GetCartItemById(Cart cart, int itemId)
    {
        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == itemId);

        if (cartItem == null)
        {
            throw new NotFoundException($"CartItem with Id {itemId} not found in the cart");
        }

        return cartItem;
    }

    private async Task<Product> GetProductForCartItem(CartItem cartItem, CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.GetRepository<Product>();
        var product = await productRepository.GetByIdAsync(cartItem.ProductId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException($"Product with Id {cartItem.ProductId} not found");
        }

        return product;
    }

    private void UpdateProductStock(Product product, int quantity)
    {
        product.StockQuantity += quantity;
    }

    private void RemoveCartItemFromCart(Cart cart, CartItem cartItem)
    {
        cart.Items.Remove(cartItem);
        cart.LastModified = DateTime.UtcNow;
    }
}
