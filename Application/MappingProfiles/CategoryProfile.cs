using AutoMapper;
using Application.Features.Products.Commands.AddProduct;
using Domain.Entities;
using Application.Common.Models;
using Application.Features.Categories.Commands.AddCategoryCommand;

namespace Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<AddCategoryDto, AddCategoryCommand>();
            CreateMap<AddCategoryCommand, Category>();
        }
    }
}
