using Application.Features.Carts.Commands.AddToCart;
using Application.Features.Carts.Commands.UpdateCartItemQuantity;
using AutoMapper;

namespace Application.MappingProfiles1
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<AddToCartDto, AddToCartCommand>();
            CreateMap<AddToCartDto, AddToCartCommand>();
            CreateMap<UpdateCartItemQuantityDto, UpdateCartItemQuantityCommand>();
        }
    }
}
