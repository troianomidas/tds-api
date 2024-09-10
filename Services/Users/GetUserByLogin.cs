using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Users;

public record GetUserByExternalIdRequest : IRequest<User?>
{
    public string? ExternalId { get; init; }
}

public class GetUserByExternalIdRequestHandler : IRequestHandler<GetUserByExternalIdRequest, User?>
{
    private readonly AppDbContext _context;

    public GetUserByExternalIdRequestHandler(AppDbContext context) => _context = context;

    public async Task<User?> Handle(GetUserByExternalIdRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ExternalId))
            return null;
        
        return await _context.Users
            .Where(x => x.ExternalId == request.ExternalId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}