using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;
public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string ShippingMethod { get; set; }
    public string ShippingAddress { get; set; }
    public Location Location { get; set; }
    public string PaymentMethod { get; set; }
    public Guid UserId { get; set; }

    public virtual List<OrderItem>? OrderItems { get; set; }

    public Order()
    {
        OrderItems = new List<OrderItem>();
    }
}
