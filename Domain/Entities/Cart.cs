using Domain.Common;

namespace Domain.Entities;
public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    public virtual decimal Total
    {
        get { return Items.Sum(item => item.DiscountedSubtotal); }
    }
    public virtual decimal TotalDiscount
    {
        get { return Items.Sum(item => item.Subtotal - item.DiscountedSubtotal); }
    }
    public Cart() { }
    public Cart(Guid userId)
    {
        UserId = userId;
        LastModified = DateTime.UtcNow;
        Created = DateTime.UtcNow;
    }
    public virtual User User { get; set; }
}
