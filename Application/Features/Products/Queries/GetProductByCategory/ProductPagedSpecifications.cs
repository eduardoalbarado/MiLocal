using Ardalis.Specification;
using Domain.Entities;

public class ProductPagedSpecifications : Specification<Product>
{
    public ProductPagedSpecifications(int pageNumber, int pageSize, bool? enabled, int? category)
    {
        Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        if (enabled.HasValue)
        {
            Query.Where(product => product.Enabled == enabled.Value);
        }

        if (category != null)
        {
            Query.Where(product => product.ProductCategories.Any(pc => pc.Category.Id == category));
        }

        Query.OrderBy(product => product.Name);
    }
}
