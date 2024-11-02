using Application.Common.Models.Responses;
using Application.Exceptions;
using Application.Features.Carts.Queries.GetCartByUserId;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Net;

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

        // Obtener el carrito del usuario
        var cartRepository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await cartRepository.FirstOrDefaultAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            throw new NotFoundException(nameof(Cart), userId);
        }

        // Obtener el item del carrito
        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == request.CartItemId);
        if (cartItem == null)
        {
            throw new NotFoundException(nameof(CartItem), request.CartItemId);
        }

        // Obtener el producto relacionado
        var productRepository = _unitOfWork.GetRepository<Product>();
        var product = await productRepository.GetByIdAsync(cartItem.ProductId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), cartItem.ProductId);
        }

        int quantityDifference = request.Quantity - cartItem.Quantity;

        // Verificar si hay stock suficiente si la cantidad aumenta
        if (quantityDifference > 0 && product.StockQuantity < quantityDifference)
        {
            throw new BusinessException("Insufficient stock for the requested quantity.", HttpStatusCode.Conflict);
        }

        // Actualizar el stock del producto según la diferencia en cantidad
        product.StockQuantity -= quantityDifference;
        await productRepository.UpdateAsync(product, cancellationToken);

        // Actualizar la cantidad en el item del carrito
        cartItem.Quantity = request.Quantity;
        cart.LastModified = DateTime.UtcNow;
        await cartRepository.UpdateAsync(cart, cancellationToken);

        // Guardar cambios
        await _unitOfWork.SaveChangesAsync();

        return Result<Unit>.Success(Unit.Value);
    }
}
