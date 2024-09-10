using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Categories;

public class UpdateCategoryRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
}

public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateCategoryRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new ProductCategory(request.StoreId, request.Name)
        {
            Status = request.Status,
            Description = request.Description
        };

        ProductCategory? categoryDb = await _context.Categories.Where(x => x.StoreId == request.StoreId && x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (categoryDb == null)
            throw new InvalidOperationException("Categoria nao encontrada");
        
        categoryDb.Name = category.Name;
        categoryDb.Description = category.Description;
        categoryDb.Status = category.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}