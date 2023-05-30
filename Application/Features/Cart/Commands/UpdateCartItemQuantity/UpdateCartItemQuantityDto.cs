namespace Application.Features.Carts.Commands.UpdateCartItemQuantity;
public class UpdateCartItemQuantityDto
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}