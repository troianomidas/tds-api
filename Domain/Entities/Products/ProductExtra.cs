using WebApi.Domain.Common;

namespace WebApi.Domain.Entities.Products;

public class ProductExtra : BaseStoreEntity
{
    public ProductExtra()
    {
        Items = new List<ProductExtraItem>();
    }

    public ProductExtra(int storeId, string? name, int min, int max, bool isRequired, ICollection<ProductExtraItem> items)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("O nome do grupo de complemento é obrigatório");

        if (name.Length is < 4 or > 60)
            throw new InvalidOperationException("O nome do grupo deve ter entre 4 e 60 caracteres");

        if (max < 1)
            throw new InvalidOperationException("A quantidade máxima do complemento é obrigatório");

        if (isRequired && min < 1)
            throw new InvalidOperationException("A quantidade mínima é obrigatória se o complemento for obrigatório");

        if (items.Count == 0)
            throw new InvalidOperationException("Adicione pelo menos 1 item no grupo de complementos");

        StoreId = storeId;
        Name = name.Trim();
        Min = min;
        Max = max;
        IsRequired = isRequired;
        Items = items;
    }

    public string? Name { get; set; }
    public int? Min { get; set; }
    public int Max { get; set; }
    public bool IsRequired { get; set; }
    public virtual ICollection<ProductExtraItem> Items { get; set; }
    public ICollection<ProductExtraMatch>? ProductExtraMatches { get; set; }
}

public class ProductExtraItem : BaseEntity
{
    public ProductExtraItem()
    {
        
    }

    public ProductExtraItem(string? name, string? description, decimal unitPrice)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("O nome do item é obrigatório");

        if (name.Length is < 4 or > 40)
            throw new InvalidOperationException("O nome do item deve ter entre 4 e 40 caracteres");
        
        if (!string.IsNullOrEmpty(description) && description.Length > 60)
            throw new InvalidOperationException("A descrição do item deve ter no máximo 60 caracteres");

        Name = name.Trim();
        Description = description;
        UnitPrice = unitPrice;
        Status = 1;
    }

    public int ProductExtraId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int Status { get; set; }
    public virtual ProductExtra? ProductExtra { get; set; }
}