using Ardalis.Specification;
using Domain.Entities;
namespace Application.Features.Carts.Queries.GetCartByUserId;
public class CartByUserIdSpecification : Specification<Cart>
{
    public CartByUserIdSpecification(Guid userId)
    {
        Query.Include(x => x.Items);
        Query.Where(cart => cart.UserId == userId);
    }
}
