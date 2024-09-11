using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
// using WebApi.Integrations.Queues;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class GetUserByLogin : IRequest<GetUserResponse>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class GetUserHandler : IRequestHandler<GetUserByLogin, GetUserResponse>
{
    private readonly AppDbContext _dbContext;

    public GetUserHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetUserResponse> Handle(GetUserByLogin request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            throw new InvalidOperationException("E-mail e/ou senha inválidos.");
        
        User? user = await _dbContext.Users.Where(x =>
            x.Email == request.Email.Trim() &&
            x.Password == request.Password).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new InvalidOperationException("E-mail e/ou senha inválidos.");

        var isVerificationRequired = false;
        
        if (user.LastAccessAt.HasValue)
        {
            if (user.LastAccessAt.Value.AddHours(6) < DateTime.Now)
                isVerificationRequired = true;
        }
        else
            isVerificationRequired = false;

        return new GetUserResponse
        {
            Success = true,
            IsVerificationRequired = false,
            User = user
        };
    }
}

public class GetUserResponse
{
    public bool Success { get; set; }
    public bool IsVerificationRequired { get; set; }
    public User? User { get; set; }
}