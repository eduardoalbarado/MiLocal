using Domain.Entities;
using Ardalis.Specification;

namespace Application.Features.Checkout.Queries.GetOrderById;
public class OrderByIdSpecification : Specification<Order>
{
    public OrderByIdSpecification(Guid userId, int orderId)
    {
        Query.Where(order => order.UserId == userId && order.Id == orderId);
    }
}
