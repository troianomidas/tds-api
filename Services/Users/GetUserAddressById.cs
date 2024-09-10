using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Users;

public record GetUserAddressById : IRequest<UserAddress?>
{
    public string? ExternalId { get; set; }
}

public class GetUserAddressByIdHandler : IRequestHandler<GetUserAddressById, UserAddress?>
{
    private readonly AppDbContext _context;

    public GetUserAddressByIdHandler(AppDbContext context) => _context = context;

    public async Task<UserAddress?> Handle(GetUserAddressById request, CancellationToken cancellationToken)
    {
        return await _context.UserAddresses.Include(x => x.User)
            .Where(x => x.User != null && x.User.ExternalId == request.ExternalId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}