using Ardalis.Specification;
using Domain.Entities;
namespace Application.Features.Carts.Queries.GetCart;
public class CartByUserIdSpecification : Specification<Cart>
{
    public CartByUserIdSpecification(Guid userId)
    {
        Query.Where(cart => cart.UserId == userId);
    }
}
