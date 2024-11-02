using Application.Common.Models.Responses;
using Application.Exceptions;
using Application.Features.Carts.Queries.GetCart;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Net;

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

            // Obtener el carrito del usuario
            var cartRepository = _unitOfWork.GetRepository<Cart>();
            var cartSpec = new CartByUserIdSpecification(userId);
            var cart = await cartRepository.FirstOrDefaultAsync(cartSpec, cancellationToken);

            if (cart == null)
            {
                throw new NotFoundException(nameof(Cart), userId);
            }

            // Obtener el item del carrito a eliminar
            var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == request.ItemId);
            if (cartItem == null)
            {
                throw new NotFoundException(nameof(cartItem), request.ItemId);
            }

            // Obtener el producto relacionado con el item del carrito
            var productRepository = _unitOfWork.GetRepository<Product>();
            var product = await productRepository.GetByIdAsync(cartItem.ProductId, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), cartItem.ProductId);
            }

            await _unitOfWork.BeginTransactionAsync();

            // Actualizar el stock del producto
            product.StockQuantity += cartItem.Quantity;
            await productRepository.UpdateAsync(product, cancellationToken);

            // Remover el item del carrito desde el repositorio de CartItem
            var cartItemRepository = _unitOfWork.GetRepository<CartItem>();
            await cartItemRepository.DeleteAsync(cartItem, cancellationToken);

            // Remover el item del carrito
            cart.LastModified = DateTime.UtcNow;
            await cartRepository.UpdateAsync(cart, cancellationToken);

            // Guardar cambios y confirmar transacción
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}
