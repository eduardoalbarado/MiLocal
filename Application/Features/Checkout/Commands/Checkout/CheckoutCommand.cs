using Application.Exceptions;
using Application.Features.Carts.Queries.GetCart;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Checkout.Commands.CreateOrder;
public class CheckoutCommand : IRequest<int>
{
}

public class CreateOrderCommandHandler : IRequestHandler<CheckoutCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<int> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);
        var userCart = await GetCartByUserId(userId, cancellationToken);

        if (userCart == null || userCart.Items.Count == 0)
        {
            throw new BadRequestException("The user's cart is empty. Cannot create an order without any items.");
        }

        var order = CreateOrder(userId, userCart);
        await _unitOfWork.GetRepository<Order>().AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        // Empty the cart items
        userCart.Items.Clear();
        await _unitOfWork.SaveChangesAsync();

        return order.Id;
    }

    private async Task<Cart?> GetCartByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await _unitOfWork.GetRepository<Cart>().GetBySpecAsync(cartSpec, cancellationToken);
        return cart;
    }

    private Order CreateOrder(Guid userId, Cart userCart)
    {
        // Map CartItem to OrderItemDto
        var orderItems = _mapper.Map<List<OrderItem>>(userCart.Items);

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            OrderItems = orderItems
        };

        // TODO: Add any additional order properties or business logic as needed.

        return order;
    }
}
