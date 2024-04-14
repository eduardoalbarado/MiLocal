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
            CreateMap<Category, CategoryDto>();
            CreateMap<AddCategoryDto, AddCategoryCommand>();
            CreateMap<AddCategoryCommand, Category>();
            CreateMap<UpdateCategoryDto, UpdateCategoryCommand>();
            CreateMap<UpdateCategoryCommand, Category>();
            CreateMap<ProductCategory, ProductCategoryGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
