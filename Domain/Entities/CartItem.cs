using Domain.Common;

namespace Domain.Entities;
public class CartItem : BaseEntity
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string? AdditionalInfo { get; set; }

    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public virtual decimal Subtotal
    {
        get { return Price * Quantity; }
    }
    public virtual decimal DiscountedSubtotal
    {
        get { return Subtotal * (1 - Discount); }
    }
    // Navigation property
    public virtual Cart Cart { get; set; }
}