using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Categories.Queries.GetCategories
{
    internal class ProductCategorySpecifications : Specification<ProductCategory>, ISingleResultSpecification<ProductCategory>
    {
        public ProductCategorySpecifications(int product, int category)
        {
            Query.Where(x => x.ProductId == product && x.CategoryId == category);
        }
    }
}
