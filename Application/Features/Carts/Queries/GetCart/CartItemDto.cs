namespace Application.Features.Carts.Queries.GetCart;
public class CartItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public virtual decimal Subtotal { get; set; }
    public virtual decimal DiscountedSubtotal { get; set; }

}
