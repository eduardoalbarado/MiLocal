using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class UserByIdSpec : Specification<User>
{
    public UserByIdSpec(string b2cUserId)
    {
        Query.Where(user => user.Id == Guid.Parse(b2cUserId));
    }
}
