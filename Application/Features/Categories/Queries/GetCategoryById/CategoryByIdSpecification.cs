using Ardalis.Specification;
using Domain.Entities;
namespace Application.Features.Categories.Queries.GetCategoryById;
public class CategoryByIdSpecification : Specification<Category>
{
    public CategoryByIdSpecification(int id)
    {
        Query.Include(x => x.ProductCategories).ThenInclude(ProductCategories => ProductCategories.Product);
        Query.Where(category => category.Id == id);
    }
}
