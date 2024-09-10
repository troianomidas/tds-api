using WebApi.Domain.Common;

namespace WebApi.Domain.Entities.Products;

public class ProductExtraMatch : BaseEntity
{
    public ProductExtraMatch()
    {
        
    }

    public ProductExtraMatch(int productId, int productExtraId)
    {
        ProductId = productId;
        ProductExtraId = productExtraId;
    }
    
    public int ProductId { get; set; }
    public int ProductExtraId { get; set; }
    
    public virtual Product? Product { get; set; }
    public virtual ProductExtra? ProductExtra { get; set; }
}