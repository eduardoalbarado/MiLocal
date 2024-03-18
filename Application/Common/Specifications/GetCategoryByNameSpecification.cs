using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;
public class GetCategoryByNameSpecification : Specification<Category>
{
    public GetCategoryByNameSpecification(string name)
    {
        Query.Where(category => category.Name == name);
    }
}