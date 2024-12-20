namespace Application.Common.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductCategoryGetProductsDto> Products { get; set; }
    }
}
