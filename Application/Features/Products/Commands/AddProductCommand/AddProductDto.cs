namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductDto
    {
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortName { get; set; }
    }
}