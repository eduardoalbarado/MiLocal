using Application.Common.Models;
using Application.Features.Products.Commands.AddProduct;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<AddProductCommand, Product>();
            CreateMap<AddProductDto, AddProductCommand>();
        }
    }
}
