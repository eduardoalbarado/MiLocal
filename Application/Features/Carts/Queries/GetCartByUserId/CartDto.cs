namespace Application.Features.Carts.Queries.GetCartByUserId;
public class CartDto
{
    public int UserId { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalPrice { get; set; }
}
