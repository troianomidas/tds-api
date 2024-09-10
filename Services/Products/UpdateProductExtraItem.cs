using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Products;

public record UpdateProductExtraItemRequest : IRequest<int>
{
    public int StoreId { get; set; }
    public int Id { get; set; }
    public int Status { get; set; }
}

public class UpdateProductExtraItemRequestHandler : IRequestHandler<UpdateProductExtraItemRequest, int>
{
    private readonly AppDbContext _context;

    public UpdateProductExtraItemRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateProductExtraItemRequest request, CancellationToken cancellationToken)
    {
        ProductExtraItem? extraItem = await _context.ProductExtraItems.Where(x => x.Id == request.Id)
            .Include(x=> x.ProductExtra)
            .FirstOrDefaultAsync(cancellationToken);
        if (extraItem == null)
            throw new BadRequestException("Complemento não encontrado.");

        if (extraItem.ProductExtra?.StoreId != request.StoreId)
            throw new BadRequestException("Lojas inválidas.");

        extraItem.Status = request.Status;
        return await _context.SaveChangesAsync(cancellationToken);
    }
}