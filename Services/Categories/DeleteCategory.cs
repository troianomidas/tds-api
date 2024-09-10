using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Categories;

public class DeleteCategoryRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public int Id { get; set; }
}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, bool>
{
    private readonly AppDbContext _context;

    public DeleteCategoryRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        ProductCategory category = await _context.Categories
            .Include(x=> x.Products)
            .ThenInclude(x=> x.Availabilities)
            .Include(x=> x.Products)
            .ThenInclude(x=> x.ProductExtraMatches)
            .FirstAsync(x => x.Id == request.Id && x.StoreId == request.StoreId, cancellationToken);

        _context.Categories.Remove(category);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}