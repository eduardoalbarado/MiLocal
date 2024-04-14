using Application.Common.Models;
using Application.Features.Categories.Commands.AddCategoryCommand;
using Application.Features.Categories.Commands.UpdateCategoryCommand;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.ToList() : null));
            CreateMap<AddCategoryDto, AddCategoryCommand>();
            CreateMap<AddCategoryCommand, Category>();
            CreateMap<UpdateCategoryDto, UpdateCategoryCommand>();
            CreateMap<UpdateCategoryCommand, Category>();
            CreateMap<ProductCategory, ProductCategoryGetCategoriesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ProductCategory, ProductCategoryGetProductsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
