using Application.Common.Models.Responses;
using Application.Exceptions;
using Application.Features.Carts.Queries.GetCartByUserId;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.Features.Carts.Commands.AddToCart;
public class AddToCartCommand : AddToCartDto, IRequest<Result<int>>
{
}

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<int>>
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

    public async Task<Result<int>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await GetProductById(request.ProductId);
        var cart = await GetOrCreateCart(cancellationToken);
        var cartItem = CreateCartItem(product, request.Quantity);

        if (product.StockQuantity < cartItem.Quantity)
        {
            throw new BusinessException("Insufficient stock for the requested product.", HttpStatusCode.Conflict);
        }

        var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (existingItem is not null)
        {
            cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId).Quantity += request.Quantity;
        }
        else
        {
            cart.Items.Add(cartItem);
        }

        cart.LastModified = DateTime.UtcNow;
        product.StockQuantity -= cartItem.Quantity;

        await UpdateCart(cart, cancellationToken);
        await UpdateProduct(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result<int>.Success(cartItem.Id);
    }

    private async Task<Product> GetProductById(int productId)
    {
        var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);

        if (product == null)
        {
            throw new NotFoundException(nameof(Product), productId);
        }

        return product;
    }

    private async Task<Cart> GetOrCreateCart(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_userContextService.GetUserContext().UserId);
        var cartRepository = _unitOfWork.GetRepository<Cart>();
        var cartSpecification = new CartByUserIdSpecification(userId);
        var cart = await cartRepository.FirstOrDefaultAsync(cartSpecification, cancellationToken);

        if (cart == null)
        {
            cart = CreateCart(userId);

            await cartRepository.AddAsync(cart, cancellationToken);
        }

        return cart;
    }

    private async Task UpdateCart(Cart cart, CancellationToken cancellationToken)
    {
        var cartRepository = _unitOfWork.GetRepository<Cart>();
        await cartRepository.UpdateAsync(cart, cancellationToken);
    }

    private async Task UpdateProduct(Product product, CancellationToken cancellationToken)
    {
        var cartProduct = _unitOfWork.GetRepository<Product>();
        await cartProduct.UpdateAsync(product, cancellationToken);
    }

    private static CartItem CreateCartItem(Product product, int quantity)
    {
        return new CartItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = quantity,
            Price = product.Price,
            Created = DateTime.UtcNow
        };
    }

    private static Cart CreateCart(Guid userId)
    {
        return new Cart(userId);
    }
}
