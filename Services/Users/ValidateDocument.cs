using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;

namespace WebApi.Services.Users;

public record ValidateDocumentRequest : IRequest<bool?>
{
    public string? Document { get; init; }
}

public class ValidateDocumentRequestHandler : IRequestHandler<ValidateDocumentRequest, bool?>
{
    private readonly AppDbContext _context;

    public ValidateDocumentRequestHandler(AppDbContext context) => _context = context;

    public async Task<bool?> Handle(ValidateDocumentRequest request, CancellationToken cancellationToken)
    {
        if (!CpfCnpj.IsCpfValid(request.Document) && !CpfCnpj.IsCnpjValid(request.Document))
            throw new InvalidOperationException("CPF ou CNPJ não é válido.");

        return true;
    }
}