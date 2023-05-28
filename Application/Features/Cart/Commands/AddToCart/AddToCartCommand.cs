using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Carts.Commands.AddToCart;

public class AddToCartCommand : IRequest<int>
{
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddToCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<CartItem>();

        // Retrieve the product from the database using the provided ProductId
        var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(request.ProductId);

        if (product == null)
        {
            // Handle the scenario where the product doesn't exist
            throw new NotFoundException("Product not found.", request.ProductId);
        }

        var cartRepository = _unitOfWork.GetRepository<Cart>();
        var cartSpec = new CartByUserIdSpecification(request.UserId);
        var cart = await cartRepository.GetBySpecAsync(cartSpec, cancellationToken);

        if (cart == null)
        {
            // Create a new cart for the user if it doesn't exist
            cart = new Cart(request.UserId);
            await cartRepository.AddAsync(cart, cancellationToken);
        }

        // Create a new cart item and map the data from the request
        var cartItem = new CartItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = request.Quantity,
            Price = product.Price
        };

        cart.Items.Add(cartItem);

        await _unitOfWork.SaveChangesAsync();

        return cartItem.Id;
    }
}
