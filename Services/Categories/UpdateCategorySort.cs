using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Categories;

public class UpdateCategorySortRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public List<CategorySort>? Categories { get; set; }

    public class CategorySort
    {
        public int Id { get; set; }
        public int Sort { get; set; }
    }
}

public class UpdateCategorySortRequestHandler : IRequestHandler<UpdateCategorySortRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateCategorySortRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCategorySortRequest request, CancellationToken cancellationToken)
    {
        if (request.Categories == null)
            return false;
        
        List<ProductCategory> categories = await _context.Categories.Where(x => x.StoreId == request.StoreId)
            .ToListAsync(cancellationToken);

        foreach (UpdateCategorySortRequest.CategorySort category in request.Categories)
            categories.First(x => x.Id == category.Id).Sort = category.Sort;

        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}