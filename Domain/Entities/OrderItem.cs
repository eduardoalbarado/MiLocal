namespace Domain.Entities;
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string ProductVariant { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public DateTime CreatedAt { get; set; } // The timestamp when the order item was created
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
    public OrderItem()
    {
        CreatedAt = DateTime.UtcNow;
    }
    public void ApplyDiscount(decimal discountPercentage)
    {
        // Apply a discount to the unit price based on the provided discount percentage
        decimal discountAmount = UnitPrice * (discountPercentage / 100);
        UnitPrice -= discountAmount;
    }
}
