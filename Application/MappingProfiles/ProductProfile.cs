using Application.Common.Models;
using Application.Features.Products.Commands.AddProduct;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.ToList() : null));
        CreateMap<AddProductCommand, Product>();
        CreateMap<AddProductDto, AddProductCommand>();
    }
}
