using Application.Features.Carts.Commands.AddToCart;
using Application.Features.Carts.Commands.UpdateCartItemQuantity;
using Application.Features.Carts.Queries.GetCart;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<AddToCartDto, AddToCartCommand>();
            CreateMap<Cart, CartDto>();
            CreateMap<UpdateCartItemQuantityDto, UpdateCartItemQuantityCommand>();
        }
    }
}
