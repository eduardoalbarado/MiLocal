using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications
{
    public class UserByB2CUserIdSpec : Specification<User>
    {
        public UserByB2CUserIdSpec(string b2cUserId)
        {
            Query.Where(user => user.B2CUserId == b2cUserId);
        }
    }
}
