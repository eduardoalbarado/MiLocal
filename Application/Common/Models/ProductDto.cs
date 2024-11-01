namespace Application.Common.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Enabled { get; set; }
        public bool Kit { get; set; }
        public List<ProductCategoryGetCategoriesDto>? Categories { get; set; }
    }
}