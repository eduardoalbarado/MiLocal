using Application.Common.Models;
using Application.Features.Carts.Commands.AddToCart;
using Application.Features.Checkout.Commands.CreateOrder;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;
public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        #region Commands
        CreateMap<AddToCartDto, AddToCartCommand>();
        CreateMap<AddToCartDto, AddToCartCommand>();
        CreateMap<CartItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
        #endregion
        #region Queries
        CreateMap<Product, ProductDto>();
        CreateMap<Order, OrderDto>();
        #endregion
    }
}
