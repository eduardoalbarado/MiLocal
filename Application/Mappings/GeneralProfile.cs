using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;
public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        #region Commands
        #endregion
        #region Queries
        CreateMap<Product, ProductDto>();
        CreateMap<Order, OrderDto>();
        #endregion
    }
}
