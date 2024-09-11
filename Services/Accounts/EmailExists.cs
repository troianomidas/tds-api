using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
// using WebApi.Integrations.Queues;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class EmailExists : IRequest<bool>
{
    public string? Email { get; set; }
}

public class EmailExistsHandler : IRequestHandler<EmailExists, bool>
{
    private readonly AppDbContext _dbContext;

    public EmailExistsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(EmailExists request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new InvalidOperationException("Preencha o e-mail.");

        return await _dbContext.Users.AnyAsync(x =>
            x.Email == request.Email.Trim(), cancellationToken: cancellationToken);
    }
}