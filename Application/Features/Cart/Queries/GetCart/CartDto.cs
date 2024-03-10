using System;

namespace Application.Features.Carts.Queries.GetCart;
public class CartDto
{
    public Guid UserId { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalPrice { get; set; }
}
