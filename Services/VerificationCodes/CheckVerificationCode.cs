// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using WebApi.Domain.Entities;
// using WebApi.Persistence;
//
// namespace WebApi.Services.VerificationCodes;
//
// public class CheckVerificationCode : IRequest<bool>
// {
//     public string? Code { get; set; }
//     public string? Email { get; set; }
// }
//
// public class CheckVerificationCodeHandler : IRequestHandler<CheckVerificationCode, bool>
// {
//     private readonly AppDbContext _dbContext;
//
//     public CheckVerificationCodeHandler(AppDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }
//
//     public async Task<bool> Handle(CheckVerificationCode request, CancellationToken cancellationToken)
//     {
//         VerificationCode? verificationCode = await _dbContext.VerificationCodes
//             .Where(x => x.Email == request.Email && x.Code == request.Code)
//             .FirstOrDefaultAsync(cancellationToken);
//
//         if (verificationCode == null)
//             throw new InvalidOperationException("Código inválido ou expirado.");
//         
//         if(verificationCode.ValidUntil < DateTime.Now)
//             throw new InvalidOperationException("Código inválido ou expirado.");
//
//         return true;
//     }
// }