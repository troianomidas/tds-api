using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Domain.Exceptions;

namespace WebApi.Services.Products;

public record DuplicateProductRequest : IRequest<int>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
}

public class DuplicateProductRequestHandler : IRequestHandler<DuplicateProductRequest, int>
{
    private readonly AppDbContext _context;

    public DuplicateProductRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DuplicateProductRequest request, CancellationToken cancellationToken)
    {
        Product? productDb = await _context.Products
            .Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .Include(x => x.Availabilities)
            .Include(x => x.ProductExtraMatches)!
            .ThenInclude(x => x.ProductExtra)
            .ThenInclude(x=> x!.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (productDb == null)
            throw new InvalidOperationException("Produto nao encontrado");

        productDb.Id = 0;
        productDb.Name += " duplicado";
        productDb.Availabilities?.ToList().ForEach(x=> x.Id = 0);
        productDb.ProductExtraMatches?.ToList().ForEach(x=> x.Id = 0);
        
        _context.Products.Add(productDb);
        await _context.SaveChangesAsync(cancellationToken);
        return productDb.Id;
    }
}