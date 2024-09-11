using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Persistence;

namespace WebApi.Services.ReferralCodes;

public record GetReferralCode : IRequest<ReferralCode?>
{
    public string? Code { get; set; }
}

public class GetReferralCodeHandler : IRequestHandler<GetReferralCode, ReferralCode?>
{
    private readonly AppDbContext _context;

    public GetReferralCodeHandler(AppDbContext context) => _context = context;

    public async Task<ReferralCode?> Handle(GetReferralCode request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.Code))
            throw new InvalidOperationException("Insira um código de cupom válido.");
        
        ReferralCode? referralCode = await _context.ReferralCodes.Where(x => x.Code!.ToLower() == request.Code.ToLower().Trim()).FirstOrDefaultAsync(cancellationToken);
        if(referralCode == null)
            throw new InvalidOperationException("Insira um código de cupom válido.");

        return referralCode;
    }
}