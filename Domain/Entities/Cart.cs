using Domain.Common;

namespace Domain.Entities;
public class Cart : BaseEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public virtual ICollection<CartItem> Items { get; set; }
    
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
