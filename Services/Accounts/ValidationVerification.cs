using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class ValidationVerification : IRequest<bool>
{
    public string? Code { get; set; }
    public string? WhatsappNumber { get; set; }
}

public class ValidationVerificationHandler : IRequestHandler<ValidationVerification, bool>
{
    private readonly AppDbContext _dbContext;

    public ValidationVerificationHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ValidationVerification request, CancellationToken cancellationToken)
    {
        WhatsappVerification? verificationCode = await _dbContext.WhatsappVerifications
            .Where(x => x.WhatsappNumber == request.WhatsappNumber && x.Code == request.Code)
            .FirstOrDefaultAsync(cancellationToken);

        if (verificationCode == null)
            throw new InvalidOperationException("O código informado está inválido.");
        
        if(verificationCode.ValidUntil < DateTime.Now)
            throw new InvalidOperationException("O código informado expirou. Tente novamente.");

        return true;
    }
}