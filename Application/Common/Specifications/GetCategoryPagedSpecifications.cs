using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.Categories.Queries.GetCategories
{
    internal class GetCategoryPagedSpecifications : Specification<Category>
    {
        public GetCategoryPagedSpecifications(int pageNumber, int pageSize, string name)
        {
            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);

            if (!string.IsNullOrEmpty(name))
                Query.Search(x => x.Name, "%" + name + "%");

            Query.OrderBy(x => x.Id);
        }
    }
}
