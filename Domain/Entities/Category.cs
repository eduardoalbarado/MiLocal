using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
}
