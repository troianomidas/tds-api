using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Messages;

namespace WebApi.Services.Products;

public record CreateOrUpdateProductRequest : IRequest<int>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? BarcodeEan { get; set; }
    public string? Sku { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsProductIndustrialized { get; set; }
    public bool Stockable { get; set; }
    public int? AvailableStock { get; set; }
    public int DiscountType { get; set; }
    public decimal Discount { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Weight { get; set; }
    public string? WeightType { get; set; }
    public int ServesHowManyPeople { get; set; }
    public int Status { get; set; }
    public bool Highlights { get; set; }
    public bool HasAvailability { get; set; }
    public bool HasExtra { get; set; }
    public List<ProductAvailability>? Availabilities { get; set; }
    public List<ProductExtraMatch>? ProductExtraMatches { get; set; }
}

public class CreateOrUpdateProductRequestHandler : IRequestHandler<CreateOrUpdateProductRequest, int>
{
    private readonly AppDbContext _context;

    public CreateOrUpdateProductRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrUpdateProductRequest request, CancellationToken cancellationToken)
     {
        Product product = ParseProductRequest(request);

        if (request.Id > 0)
        {
            Product? productDb = await _context.Products
                .Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
                .Include(x => x.Availabilities)
                .Include(x => x.ProductExtraMatches)
                .FirstOrDefaultAsync(cancellationToken);

            if (productDb == null)
                throw new InvalidOperationException("Produto não encontrado.");

            productDb.Name = product.Name;
            productDb.ImageUrl = product.ImageUrl;
            productDb.IsProductIndustrialized = product.IsProductIndustrialized;
            productDb.Description = product.Description;
            productDb.Sku = product.Sku;
            productDb.BarcodeEan = product.BarcodeEan;
            productDb.CategoryId = product.CategoryId;
            productDb.ServesHowManyPeople = product.ServesHowManyPeople;
            productDb.Weight = product.Weight;
            productDb.Highlights = product.Highlights;
            productDb.WeightType = product.WeightType;
            productDb.UnitPrice = product.UnitPrice;
            productDb.Discount = product.Discount;
            productDb.DiscountType = product.DiscountType;
            productDb.Stockable = product.Stockable;
            productDb.AvailableStock = product.AvailableStock;
            productDb.HasAvailability = product.HasAvailability;
            productDb.Availabilities = product.Availabilities;
            productDb.HasExtra = product.HasExtra;
            productDb.ProductExtraMatches = product.ProductExtraMatches;

            productDb.Status = product.Status;
        }
        else
            _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    private static Product ParseProductRequest(CreateOrUpdateProductRequest request)
    {
        request.ProductExtraMatches?.ForEach(x => x.ProductExtra = null);

        var product = new Product(request.StoreId, request.Name, request.CategoryId, request.UnitPrice)
        {
            IsProductIndustrialized = request.IsProductIndustrialized,
            Description = request.Description,
            Sku = request.Sku,
            BarcodeEan = request.BarcodeEan,
            ImageUrl = request.ImageUrl,
            ServesHowManyPeople = request.ServesHowManyPeople,
            Status = request.Status,
            Highlights = request.Highlights
        };

        if (!string.IsNullOrEmpty(product.Description) && product.Description.Length is < 4 or > 600)
            throw new InvalidOperationException("Descrição do produto deve ter entre 4 e 600 caracteres.");

        if (product.IsProductIndustrialized && string.IsNullOrEmpty(product.BarcodeEan))
            throw new InvalidOperationException("Informe o Código de barras (EAN) do produto.");

        product.SetDiscount(request.DiscountType, request.Discount);
        product.SetWeight(request.Weight, request.WeightType);
        product.SetStock(request.Stockable, request.AvailableStock ?? 0);
        product.SetAvailability(request.HasAvailability, request.Availabilities);
        product.SetExtra(request.HasExtra, request.ProductExtraMatches);

        return product;
    }
}