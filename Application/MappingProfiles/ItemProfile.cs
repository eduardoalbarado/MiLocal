using Application.Features.Carts.Queries.GetCart;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles1
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<CartItem, CartItemDto>();
        }
    }
}
