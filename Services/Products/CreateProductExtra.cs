using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;

namespace WebApi.Services.Products;

public record CreateProductExtraRequest : IRequest<int>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public bool IsRequired { get; set; }
    public List<ProductExtraItemRequest>? Items { get; set; }
}

public record ProductExtraItemRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateProductExtraRequestHandler : IRequestHandler<CreateProductExtraRequest, int>
{
    private readonly AppDbContext _context;

    public CreateProductExtraRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductExtraRequest request, CancellationToken cancellationToken)
    {
        var items = new List<ProductExtraItem>();

        if (request.Items == null || request.Items.Count == 0)
            throw new BadRequestException("Adicione 1 item no grupo de complementos");

        foreach (ProductExtraItemRequest item in request.Items)
        {
            try
            {
                items.Add(new ProductExtraItem(item.Name, item.Description, item.UnitPrice)
                {
                    ImageUrl = item.ImageUrl
                });
            }
            catch (InvalidOperationException e)
            {
                throw new BadRequestException(e.Message);
            }
        }

        try
        {
            var extra = new ProductExtra(request.StoreId, request.Name, request.Min, request.Max, request.IsRequired,
                items);
            _context.ProductExtras.Add(extra);
            await _context.SaveChangesAsync(cancellationToken);
            return extra.Id;
        }
        catch (InvalidOperationException e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}