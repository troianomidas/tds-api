using WebApi.Domain.Common;
using WebApi.Domain.Entities.Stores;

namespace WebApi.Domain.Entities.Products;

public class ProductCategory : BaseStoreEntity
{
    public ProductCategory(int storeId, string? name)
    {
        if(storeId < 1)
            throw new InvalidOperationException("Código da loja é obrigatório");
        
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Informe o nome da categoria");

        if (name.Length is < 4 or > 45)
            throw new InvalidOperationException("O nome da categoria ter entre 4 e 45 caracteres");

        Name = name.Trim();
        StoreId = storeId;
        Status = 1;
        Sort = 999;
        Products = new List<Product>();
    }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public int Sort { get; set; }
    public ICollection<Product> Products { get; set; }
    public virtual Store? Store { get; set; }
}