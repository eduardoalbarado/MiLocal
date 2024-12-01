using System;

namespace Application.Features.Carts.Queries.GetCart;
public class CartDto
{
    public Guid UserId { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalPrice => CalculateTotalPrice();
    private decimal CalculateTotalPrice()
    {
        decimal totalPrice = 0;

        if (Items != null)
        {
            foreach (var item in Items)
            {
                totalPrice += item.DiscountedSubtotal;
            }
        }

        return totalPrice;
    }
}
