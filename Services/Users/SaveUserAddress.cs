using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Users;

public record SaveUserAddress : IRequest<bool>
{
    public string? ExternalId { get; set; }
    public string? Zipcode { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityState { get; set; }
}

public class SaveUserAddressHandler : IRequestHandler<SaveUserAddress, bool>
{
    private readonly AppDbContext _context;

    public SaveUserAddressHandler(AppDbContext context) => _context = context;

    public async Task<bool> Handle(SaveUserAddress request, CancellationToken cancellationToken)
    {
        User? userDb = await _context.Users.Where(x => x.ExternalId == request.ExternalId)
            
            .FirstOrDefaultAsync(cancellationToken);
        if (userDb == null)
            throw new InvalidOperationException("Usuario nao encontrado.");

        var address = new UserAddress(request.Zipcode, request.Line1, request.Number, request.Neighborhood, request.CityState)
        {
            UserId = userDb.Id,
            Line2 = request.Line2
        };
        
        // if (userDb.Address != null)
        // {
        //     userDb.Address.Zipcode = address.Zipcode;
        //     userDb.Address.Line1 = address.Line1;
        //     userDb.Address.Line2 = address.Line2;
        //     userDb.Address.Number = address.Number;
        //     userDb.Address.Neighborhood = address.Neighborhood;
        //     userDb.Address.CityState = address.CityState;
        //     userDb.Address.CreatedAt = DateTime.Now;
        // }
        // else
        // {
        //     userDb.Address = address;
        // }
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}