using Application.Common.Models;
using Application.Common.Models.Checkout;
using Application.Exceptions;
using Application.Features.Carts.Queries.GetCart;
using Application.Interfaces;
using Application.Interfaces.PaymentService;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Checkout.Commands.StartCheckout;

public class StartCheckoutCommand : StartCheckoutRequestDto, IRequest<int>
{
}

public class StartCheckoutCommandHandler : IRequestHandler<StartCheckoutCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IPaymentGatewayService _paymentService;

    public StartCheckoutCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService,
        IPaymentGatewayService paymentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
        _paymentService = paymentService;
    }

    public async Task<int> Handle(StartCheckoutCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);
        var userCart = await GetCartByUserId(userId, cancellationToken);

        if (userCart == null || userCart.Items.Count == 0)
        {
            throw new BadRequestException("The user's cart is empty. Cannot create an order without any items.");
        }

        // Lógica para crear la orden preliminar
        var order = CreateOrder(userId, userCart, request);
        await _unitOfWork.GetRepository<Order>().AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        // Proceso de pago usando el servicio de pago
        var paymentResult = await _paymentService.ProcessPayment(order);

        if (!paymentResult.IsSuccess)
        {
            throw new Exception("Payment failed: " + paymentResult.IsSuccess);
        }

        // Vaciar el carrito después del pago exitoso
        userCart.Items.Clear();
        await _unitOfWork.SaveChangesAsync();

        return order.Id;
    }

    private async Task<Cart?> GetCartByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await _unitOfWork.GetRepository<Cart>().FirstOrDefaultAsync(cartSpec, cancellationToken);
        return cart;
    }

    private Order CreateOrder(Guid userId, Cart userCart, StartCheckoutCommand request)
    {
        var orderItems = _mapper.Map<List<OrderItem>>(userCart.Items);

        var order = new Order
        {
            UserId = userCart.UserId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = 1000,
            CustomerName = _userContextService.GetUserContext().UserName,
            CustomerEmail = _userContextService.GetUserContext().Email,
            ShippingMethod = request.ShippingMethod,
            ShippingAddress = request.ShippingAddress,
            Location = request.Location,
            PaymentMethod = string.Empty
            //OrderItems = null
        };
        // Set each OrderItem's Order reference
        foreach (var item in orderItems)
        {
            item.OrderId = 0;
            item.Id = 0;
        }
        return order;
    }
}
