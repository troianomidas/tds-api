using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Integrations.Queues;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class PreRegister : IRequest<string?>
{
    public string? Username { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? Pass1 { get; set; }
    public string? Pass2 { get; set; }
    public string? PublicIp { get; set; }
    public DateTime Now { get; set; } = DateTime.Now;
}

public class PreRegisterHandler : IRequestHandler<PreRegister, string?>
{
    private readonly AppDbContext _dbContext;
    private readonly IQueue _queue;

    public PreRegisterHandler(AppDbContext dbContext, IQueue queue)
    {
        _dbContext = dbContext;
        _queue = queue;
    }

    public async Task<string?> Handle(PreRegister request, CancellationToken cancellationToken)
    {
        bool userAlreadyExists =
            await _dbContext.Users.AnyAsync(x => x.WhatsappNumber == request.WhatsappNumber, cancellationToken);

        if (userAlreadyExists)
            throw new InvalidOperationException("Esse número de telefone já está vinculado a uma conta Quantso.");

        var user = new User(request.Username, request.WhatsappNumber, request.Pass1, request.Pass2);

        var verification = new WhatsappVerification("register", request.WhatsappNumber)
        {
            PublicIp = request.PublicIp
        };

        int attemptedCount = await _dbContext.WhatsappVerifications.CountAsync(x => x.PublicIp == request.PublicIp && x.ValidationType == "register" && x.CreatedAt >= request.Now.AddHours(-3), cancellationToken);
        if (attemptedCount > 3)
            throw new InvalidOperationException("Você atingiu o limite de tentativas. Por favor, tente novamente em algumas horas.");

        _dbContext.WhatsappVerifications.Add(verification);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //await _queue.SendMessageAsync(QueueConst.SendWhatsappMessageQueue, JsonConvert.SerializeObject(verification));

        return user.ExternalId;
    }
}