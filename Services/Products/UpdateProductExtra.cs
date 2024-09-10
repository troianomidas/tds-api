using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace WebApi.Services.Products;

public record UpdateProductExtraRequest : IRequest<int>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public bool IsRequired { get; set; }
    public List<UpdateProductExtraItem>? Items { get; set; }
}

public record UpdateProductExtraItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Status { get; set; }
}

public class UpdateProductExtraRequestHandler : IRequestHandler<UpdateProductExtraRequest, int>
{
    private readonly AppDbContext _context;

    public UpdateProductExtraRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateProductExtraRequest request, CancellationToken cancellationToken)
    {
        var items = new List<ProductExtraItem>();

        if (request.Items == null || request.Items.Count == 0)
            throw new BadRequestException("Adicione 1 item no grupo de complementos.");

        foreach (UpdateProductExtraItem item in request.Items)
        {
            try
            {
                items.Add(new ProductExtraItem(item.Name, item.Description, item.UnitPrice)
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Status = item.Status
                });
            }
            catch (InvalidOperationException e)
            {
                throw new BadRequestException(e.Message);
            }
        }

        ProductExtra? productExtra = await _context.ProductExtras.Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if(productExtra == null)
            throw new BadRequestException("Complemento não encontrado.");

        productExtra.Name = request.Name;
        productExtra.Min = request.Min;
        productExtra.Max = request.Max;
        productExtra.IsRequired = request.IsRequired;
        productExtra.Items = items;
        
        await _context.SaveChangesAsync(cancellationToken);
        return productExtra.Id;
    }
}