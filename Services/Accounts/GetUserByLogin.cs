using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Integrations.Queues;
using WebApi.Persistence;

namespace WebApi.Services.Accounts;

public class GetUserByLogin : IRequest<GetUserResponse>
{
    public string? WhatsappNumber { get; set; }
    public string? Password { get; set; }
}

public class GetUserHandler : IRequestHandler<GetUserByLogin, GetUserResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly IQueue _queue;

    public GetUserHandler(AppDbContext dbContext, IQueue queue)
    {
        _dbContext = dbContext;
        _queue = queue;
    }

    public async Task<GetUserResponse> Handle(GetUserByLogin request, CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.Where(x =>
            x.WhatsappNumber == request.WhatsappNumber &&
            x.Password == request.Password).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new InvalidOperationException("Número de WhatsApp e/ou senha inválidos");

        var resp = new GetUserResponse
        {
            Success = true,
            UserExternalId = user.ExternalId
        };
        
        if (user.LastAccessAt!.Value.AddHours(6) < DateTime.Now)
        {   
            var verification = new WhatsappVerification("login", request.WhatsappNumber);
            
            //await _queue.SendMessageAsync(QueueConst.SendWhatsappMessageQueue, JsonConvert.SerializeObject(verification));

            resp.IsVerificationRequired = true;
            _dbContext.WhatsappVerifications.Add(verification);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return resp;
    }
}

public class GetUserResponse
{
    public bool Success { get; set; }
    public bool IsVerificationRequired { get; set; }
    public string? UserExternalId { get; set; }
}