using Application.Features.Products.Queries.GetProducts;
using AutoMapper;
using Domain.Entities;

namespace Noname.Application.Mappings;
public class GeneraProfile : Profile
{
    public GeneraProfile()
    {
        #region Commands
        //
        #endregion
        #region Queries
        CreateMap<Product, ProductDto>();
        #endregion
    }
}
