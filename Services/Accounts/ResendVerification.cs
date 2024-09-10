using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Integrations.Queues;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class ResendVerification : IRequest<bool>
{
    public string? WhatsappNumber { get; set; }
    public DateTime Now { get; set; } = DateTime.Now;
}

public class ResendVerificationHandler : IRequestHandler<ResendVerification, bool>
{
    private readonly AppDbContext _dbContext;
    private readonly IQueue _queue;

    public ResendVerificationHandler(AppDbContext dbContext, IQueue queue)
    {
        _dbContext = dbContext;
        _queue = queue;
    }

    public async Task<bool> Handle(ResendVerification request, CancellationToken cancellationToken)
    {
        int verificationCount = await _dbContext.WhatsappVerifications
            .CountAsync(x => x.ValidationType == "resend" && x.WhatsappNumber == request.WhatsappNumber &&
                             x.CreatedAt >= request.Now.Date && x.CreatedAt <= request.Now.Date.AddDays(1),
                cancellationToken: cancellationToken);

        if (verificationCount > 1)
            throw new InvalidOperationException(
                "Você atingiu o limite de tentativas. Por favor, tente novamente em algumas horas.");

        var verification = new WhatsappVerification("resend", request.WhatsappNumber);
        
        await _queue.SendMessageAsync(QueueConst.SendWhatsappMessageQueue, JsonConvert.SerializeObject(verification));

        _dbContext.WhatsappVerifications.Add(verification);
        
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}