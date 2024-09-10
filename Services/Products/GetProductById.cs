using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Products;

public record GetProductByIdRequest : IRequest<Product?>
{
    public int StoreId { get; set; }
    public int ProductId { get; init; }
}

public class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, Product?>
{
    private readonly AppDbContext _context;

    public GetProductByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(x => x.Id == request.ProductId && x.StoreId == request.StoreId)
            .Include(x => x.Availabilities)
            .Include(x => x.ProductExtraMatches)!
            .ThenInclude(x => x.ProductExtra)
            .ThenInclude(x=> x!.Items)
            .FirstOrDefaultAsync(cancellationToken);
    }
}