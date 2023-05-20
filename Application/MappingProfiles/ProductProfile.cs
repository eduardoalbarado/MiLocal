using AutoMapper;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Queries.GetProducts;
using Domain.Entities;

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
