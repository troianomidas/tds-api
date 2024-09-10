using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Products;

public class DeleteProductRequest : IRequest<bool>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
}

public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, bool>
{
    private readonly AppDbContext _context;

    public DeleteProductRequestHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        Product? productDb = await _context.Products
            .Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .Include(x => x.Availabilities)
            .Include(x => x.ProductExtraMatches)!
            .ThenInclude(x => x.ProductExtra)
            .ThenInclude(x=> x!.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (productDb == null)
            throw new InvalidOperationException("Produto não encontrado.");
        
        _context.Products.Remove(productDb);
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}