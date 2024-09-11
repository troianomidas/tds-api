using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
// using WebApi.Integrations.Queues;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class DocumentExists : IRequest<bool>
{
    public string? Document { get; set; }
}

public class DocumentExistsHandler : IRequestHandler<DocumentExists, bool>
{
    private readonly AppDbContext _dbContext;

    public DocumentExistsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DocumentExists request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Document))
            throw new InvalidOperationException("Preencha CPF do responsável legal.");

        return await _dbContext.Stores.AnyAsync(x =>
            x.OwnerDocument == request.Document.Trim(), cancellationToken: cancellationToken);
    }
}