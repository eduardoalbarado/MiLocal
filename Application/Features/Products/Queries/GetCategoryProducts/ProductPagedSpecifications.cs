using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Products.Queries.GetCategoryProducts;
public class ProductPagedSpecifications : Specification<Product>
{
    public ProductPagedSpecifications(int pageNumber, int pageSize, bool? enabled, int categoryId)
    {
        Query
            .Where(p => p.ProductCategories.Any(cp => cp.CategoryId == categoryId));

        if (enabled.HasValue)
            Query.Where(p => p.Enabled == enabled);

        Query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
