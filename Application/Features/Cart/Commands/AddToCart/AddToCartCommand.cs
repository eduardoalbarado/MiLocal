using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.AddToCart;
public class AddToCartCommand : IRequest<int>
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public AddToCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<int> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await GetProductById(request.ProductId);
        var cart = await GetOrCreateCart(cancellationToken);
        var cartItem = CreateCartItem(product, request.Quantity);

        cart.Items.Add(cartItem);
        await _unitOfWork.SaveChangesAsync();

        return cartItem.Id;
    }

    private async Task<Product> GetProductById(int productId)
    {
        var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);

        if (product == null)
        {
            throw new NotFoundException("Product not found.", productId);
        }

        return product;
    }

    private async Task<Cart> GetOrCreateCart(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);
        var cartRepository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(userId);
        var cart = await cartRepository.GetBySpecAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            cart = CreateCart(userId);
            await cartRepository.AddAsync(cart, cancellationToken);
        }

        return cart;
    }

    private CartItem CreateCartItem(Product product, int quantity)
    {
        return new CartItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = quantity,
            Price = product.Price
        };
    }

    private Cart CreateCart(Guid userId)
    {
        return new Cart(userId);
    }
}
