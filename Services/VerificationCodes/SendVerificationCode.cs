// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Newtonsoft.Json;
// using WebApi.Domain.Constants;
// using WebApi.Domain.Entities;
// using WebApi.Integrations.Queues;
// using WebApi.Persistence;
//
// namespace WebApi.Services.VerificationCodes;
//
// public class SendVerificationCode : IRequest<bool>
// {
//     public string? Email { get; set; }
//     public string? Name { get; set; }
//     public string? PublicIp { get; set; }
//     public DateTime Now { get; set; } = DateTime.Now;
// }
//
// public class SendVerificationCodeHandler : IRequestHandler<SendVerificationCode, bool>
// {
//     private readonly AppDbContext _dbContext;
//     private readonly IQueue _queue;
//
//     public SendVerificationCodeHandler(AppDbContext dbContext, IQueue queue)
//     {
//         _dbContext = dbContext;
//         _queue = queue;
//     }
//
//     public async Task<bool> Handle(SendVerificationCode request, CancellationToken cancellationToken)
//     {
//         int verificationCount = await _dbContext.VerificationCodes
//             .CountAsync(x => x.PublicIp == request.PublicIp &&
//                              x.CreatedAt >= request.Now.AddHours(-1) && x.CreatedAt <= request.Now.AddHours(1),
//                 cancellationToken: cancellationToken);
//
//         if (verificationCount > 3)
//             throw new InvalidOperationException(
//                 "Você atingiu o limite de tentativas. Por favor, tente novamente em algumas horas.");
//
//         var verification = new VerificationCode(request.PublicIp, request.Email);
//         _dbContext.VerificationCodes.Add(verification);
//         
//         // await _queue.SendMessageAsync(QueueConst.SendEmailTransactionQueue, JsonConvert.SerializeObject(new
//         // {
//         //     Template = "verification_code",
//         //     Subject = $"{verification.Code} é o seu código de acesso",
//         //     Body = verification.Code,
//         //     ToEmail = request.Email,
//         //     ToName = request.Name
//         // }));
//         
//         return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
//     }
// }