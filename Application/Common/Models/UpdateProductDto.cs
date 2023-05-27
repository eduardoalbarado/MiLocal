namespace Application.Common.Models;
public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal PriceInDollars { get; set; }
    public decimal Cost { get; set; }
    public decimal CostInDollars { get; set; }
    public bool Enabled { get; set; }
    public bool Kit { get; set; }
}
