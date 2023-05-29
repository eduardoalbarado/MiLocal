using Application.Common.Models;
using Application.Features.Carts.Commands.AddToCart;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;
public class GeneraProfile : Profile
{
    public GeneraProfile()
    {
        #region Commands
        CreateMap<AddToCartDto, AddToCartCommand>();
        #endregion
        #region Queries
        CreateMap<Product, ProductDto>();
        #endregion
    }
}
