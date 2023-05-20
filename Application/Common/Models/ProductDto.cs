using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Models
{
    public class ProductDto : IMapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Enabled { get; set; }
        public bool Kit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>();
        }
    }
}