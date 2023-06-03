using Domain.Common;

namespace Domain.Entities;
public class CartItem : BaseEntity
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string AdditionalInfo { get; set; }

    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public virtual decimal Subtotal
    {
        get { return Price * Quantity; }
    }
    public decimal DiscountedSubtotal
    {
        get { return Subtotal * (1 - Discount); }
    }
}