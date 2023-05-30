using Domain.Entities;

namespace Application.Common.Models;
public class OrderDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string ShippingAddress { get; set; }
    public virtual List<OrderItem>? OrderItems { get; set; }
}
