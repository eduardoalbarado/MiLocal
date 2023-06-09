﻿using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Orders.Queries.GetOrderById;
public class OrderByIdSpecification : Specification<Order>
{
    public OrderByIdSpecification(Guid userId, int orderId)
    {
        Query.Where(order => order.UserId == userId && order.Id == orderId);
    }
}
