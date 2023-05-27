using AutoMapper;
using Application.Features.Products.Commands.AddProduct;
using Domain.Entities;
using Application.Common.Models;

namespace Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<AddProductCommand, Product>();
        }
    }
}
