using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Persistence;
using MediatR;

namespace WebApi.Services.Categories;

public class CreateCategoryRequest : IRequest<ProductCategory>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, ProductCategory>
{
    private readonly AppDbContext _context;

    public CreateCategoryRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCategory> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new ProductCategory(request.StoreId, request.Name)
        {
            Description = request.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return category;
    }
}