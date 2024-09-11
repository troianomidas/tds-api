using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Entities.Subscriptions;
// using WebApi.Integrations.Queues;
using WebApi.Services.Subscriptions;

namespace WebApi.Services.Accounts;

public record Register : IRequest<string?>
{
    public string? Username { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? Pass1 { get; set; }
    public string? Pass2 { get; set; }
    public string? PublicIp { get; set; }
}

public class RegisterHandler : IRequestHandler<Register, string?>
{
    private readonly AppDbContext _context;
    // private readonly IQueue _queue;

    // public RegisterHandler(AppDbContext context, IQueue queue)
    // {
    //     _context = context;
    //     _queue = queue;
    // }

    public RegisterHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<string?> Handle(Register request, CancellationToken cancellationToken)
    {
        return null;

        // bool userAlreadyExists =
        //     await _context.Users.AnyAsync(x => x.WhatsappNumber == request.WhatsappNumber, cancellationToken);
        //
        // if (userAlreadyExists)
        //     throw new InvalidOperationException("Esse número de telefone já está vinculado a uma conta Quantso.");
        //
        // var user = new User(request.Username, request.WhatsappNumber, request.Pass1, request.Pass2)
        // {
        //     PublicIp = request.PublicIp,
        //     LastAccessAt = DateTime.Now
        // };
        //
        // _context.Users.Add(user);
        //
        // await _context.SaveChangesAsync(cancellationToken);
        //
        // var store = new Store(user.Id, user.ExternalId ?? Guid.NewGuid().ToString());
        //
        // store.Announcements?.Add(new Announcement
        // {
        //     Title = "\ud83c\udf89 Estamos muito animados por tê-lo a bordo na nossa plataforma.",
        //     Description = $"Olá, {user.Name}!<br><br>\ud83c\udf89 Seja muito bem-vindo(a) à Quantso! \ud83c\udf89 <br><br>Estamos felizes demais por você ter escolhido a gente. Queremos que você se sinta completamente à vontade aqui, afinal, criamos a Quantso pensando em oferecer uma plataforma simples e prática para que você tenha mais agilidade nas suas vendas.<br><br>Sabemos que novos começos podem trazer algumas dúvidas. Mesmo acreditando na simplicidade e intuitividade de nossa plataforma, preparamos um vídeo introdutório para você. Esperamos que ele te ajude a ficar por dentro de todas as possibilidades dentro do nosso app.<br><br>Pronto para iniciar essa nova fase conosco?<br><br>Estamos aqui para o que você precisar.",
        //     Type = 1,
        //     Status = 1,
        //     CreatedAt = DateTime.Now
        // });
        //
        // _context.Stores.Add(store);
        //
        // await _context.SaveChangesAsync(cancellationToken);
        //
        // await _queue.CreateQueueAsync(store.ExternalId ?? string.Empty);
        //
        // return user.ExternalId;
    }
}