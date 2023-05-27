using Ardalis.Specification;
using Domain.Entities;

public class CartByUserIdSpecification : Specification<Cart>
{
    public CartByUserIdSpecification(Guid userId)
    {
        Query.Where(cart => cart.UserId == userId);
    }
}
