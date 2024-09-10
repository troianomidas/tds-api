using System.Globalization;
using WebApi.Domain.Common;
using WebApi.Domain.Entities.Stores;
using WebApi.Domain.Messages;

namespace WebApi.Domain.Entities.Products;

public class Product : BaseStoreEntity
{
    public Product()
    {
        
    }

    public Product(int storeId, string? name, int categoryId, decimal unitPrice)
    {
        if (storeId < 1)
            throw new InvalidOperationException("Informe o código da loja.");
        
        if(string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Informe o nome do produto.");
        
        if (name.Length is < 4 or > 60)
            throw new InvalidOperationException("Nome do produto deve ter entre 4 e 60 caracteres.");
        
        if (categoryId < 1)
            throw new InvalidOperationException("Selecione a categoria do produto.");
        
        if (unitPrice <= 0.0m)
            throw new InvalidOperationException(InputMsg.PriceRequired);

        StoreId = storeId;
        Name = name.Trim();
        CategoryId = categoryId;
        UnitPrice = unitPrice;
        Status = 1;
    }

    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public string? BarcodeEan { get; set; }
    public string? Sku { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public bool Stockable { get; set; }
    public int AvailableStock { get; set; }
    public int DiscountType { get; set; }
    public decimal Discount { get; set; }
    public bool IsProductIndustrialized { get; set; }
    public string? Weight { get; set; }
    public string? WeightType { get; set; }
    public int ServesHowManyPeople { get; set; }
    public bool HasAvailability { get; set; }
    public bool HasExtra { get; set; }
    public bool Highlights { get; set; }
    
    public int Views { get; set; }
    public int Status { get; set; }
    public virtual ProductCategory? Category { get; set; }
    public virtual Store? Store { get; set; }
    public ICollection<ProductAvailability>? Availabilities { get; set; }
    public virtual ICollection<ProductExtraMatch>? ProductExtraMatches { get; set; }

    public void SetAvailability(bool hasAvailability, List<ProductAvailability>? availabilities)
    {
        if(hasAvailability && availabilities?.Count == 0)
            throw new InvalidOperationException("Informe os dias e horários que o item ficará disponível.");

        if (hasAvailability && availabilities != null)
        {
            foreach (ProductAvailability item in availabilities)
            {
                DateTime beginAt = DateTime.Parse($"24/06/2023 {item.BeginAt}");
                DateTime endAt = DateTime.Parse($"24/06/2023 {item.EndAt}");
                
                if(beginAt >= endAt)
                    throw new InvalidOperationException($"O turno '{item.BeginAt}' '{item.EndAt}' da disponibilidade do item é inválido.");
            }
        }
        
        HasAvailability = hasAvailability;
        Availabilities = availabilities;
    }
    
    public void SetExtra(bool hasExtra, List<ProductExtraMatch>? extras)
    {
        if(hasExtra && extras?.Count == 0)
            throw new InvalidOperationException("Informe os complementos deste item.");

        HasExtra = hasExtra;
        ProductExtraMatches = extras;
    }
    
    public void SetDiscount(int discountType, decimal value)
    {
        if (discountType != 1 && value < 0.1m)
            throw new InvalidOperationException(InputMsg.Required, new Exception("discount"));

        if (discountType == 2 && value > 100m)
            throw new InvalidOperationException(InputMsg.Invalid, new Exception("discount"));

        if (discountType == 3 && value >= UnitPrice)
            throw new InvalidOperationException(InputMsg.Invalid, new Exception("discount"));

        DiscountType = discountType;
        Discount = value;
    }

    public void SetWeight(string? weight, string? type)
    {
        if (string.IsNullOrEmpty(weight))
            weight = "0";
        
        if (Convert.ToInt32(weight) < 0)
            throw new InvalidOperationException("Informe um valor válido para o peso do produto.");
        
        Weight = weight;
        WeightType = type;
    }

    public void SetStock(bool stockable, int availableStock)
    {
        if (stockable && availableStock < 1)
            throw new InvalidOperationException("Informe a quantidade de estoque disponível do produto.");
        
        Stockable = stockable;
        AvailableStock = availableStock;
    }
}