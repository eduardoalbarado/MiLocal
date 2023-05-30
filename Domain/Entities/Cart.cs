using Domain.Common;

namespace Domain.Entities;
public class Cart : BaseEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; }
    public decimal TotalPrice { get; set; }
    // Additional properties and methods for the shopping cart

    public Cart(Guid userId)
    {
        UserId = userId;
        Items = new List<CartItem>();
    }
}
