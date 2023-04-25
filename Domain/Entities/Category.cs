using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}