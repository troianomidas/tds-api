using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using WebApi.Services.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Accounts;

public record Login : IRequest<AuthUserResponse?>
{
    public string? UserExternalId { get; set; }
}

public class LoginHandler : IRequestHandler<Login, AuthUserResponse?>
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public LoginHandler(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthUserResponse?> Handle(Login request, CancellationToken cancellationToken)
    {
        Store? store = await _context.Stores
            .Include(x=> x.User)
            .Where(x => x.User != null && x.User.ExternalId == request.UserExternalId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (store?.User == null)
            throw new InvalidOperationException("User or Store not found");
        
        store.User.LastAccessAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);
        
        return new AuthUserResponse
        {
            BearerToken = _tokenService.GenerateJwtToken(store.User.Id, store.Id),
            Store = store
        };
    }
}

public class AuthUserResponse
{
    public string? BearerToken { get; set; }
    public Store? Store { get; set; }
}