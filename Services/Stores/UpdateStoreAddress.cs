using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record UpdateStoreAddressRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public string? Zipcode { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityState { get; set; }
}

public class UpdateStoreAddressRequestHandler : IRequestHandler<UpdateStoreAddressRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateStoreAddressRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStoreAddressRequest request, CancellationToken cancellationToken)
    {
        Store? storeDb = await _context.Stores.Where(x => x.Id == request.StoreId)
            .Include(x=> x.Address)
            .FirstOrDefaultAsync(cancellationToken);

        if (storeDb == null)
            throw new InvalidOperationException("Loja nao encontrada");

        var address = new StoreAddress(request.Zipcode, request.Line1, request.Number, request.Neighborhood, request.CityState)
        {
            Id = storeDb.Address?.Id ?? 0,
            Line2 = request.Line2
        };

        storeDb.Address = address;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}