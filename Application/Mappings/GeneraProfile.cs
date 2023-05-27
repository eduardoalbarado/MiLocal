using Application.Common.Models;
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
