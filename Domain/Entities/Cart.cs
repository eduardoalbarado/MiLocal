using Domain.Common;

namespace Domain.Entities;
public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; }
    public decimal TotalPrice { get; set; }
    public virtual decimal Total
    {
        get { return Items.Sum(item => item.DiscountedSubtotal); }
    }
    public virtual decimal TotalDiscount
    {
        get { return Items.Sum(item => item.Subtotal - item.DiscountedSubtotal); }
    }
    public Cart(Guid userId)
    {
        UserId = userId;
        Items = new List<CartItem>();
    }
}
