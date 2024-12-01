﻿namespace Application.Features.Carts.Queries.GetCartByUserId;
public class CartItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
