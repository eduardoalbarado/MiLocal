using Domain.Common;

namespace Domain.Entities;
public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string ProductVariant { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }

    public void ApplyDiscount(decimal discountPercentage)
    {
        // Apply a discount to the unit price based on the provided discount percentage
        decimal discountAmount = UnitPrice * (discountPercentage / 100);
        UnitPrice -= discountAmount;
    }
}
