using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Products.Queries.GetProductById;
internal class ProductByIdSpecifications : Specification<Product>, ISingleResultSpecification<Product>
{
    public ProductByIdSpecifications(int id)
    {
        Query.Include(product => product.ProductCategories).ThenInclude(productCategory => productCategory.Category);

        Query.Where(x => x.Id == id);
    }
}