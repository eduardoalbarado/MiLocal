using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Products.Queries.GetProducts;

internal class ProductPagedSpecifications : Specification<Product>
{
    public ProductPagedSpecifications(int pageNumber, int pageSize, bool? enabled)
    {
        Query.Include(product => product.ProductCategories).ThenInclude(productCategory => productCategory.Category);
        Query.Skip((pageNumber - 1) * pageSize)
             .Take(pageSize);

        if (enabled is not null)
            Query.Where(x => x.Enabled == enabled);

        Query.OrderBy(x => x.Id);
    }
}