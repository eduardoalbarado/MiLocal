using Domain.Common;

namespace Domain.Entities;
public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }
    public Guid Identifier { get; set; }
    public bool IsMainImage { get; set; }

    public virtual Product Product { get; set; }
}
