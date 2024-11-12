using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CartItem, OrderItem>();
    }
}
