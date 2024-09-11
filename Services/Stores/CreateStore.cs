using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Subscriptions;
// using WebApi.Integrations.Queues;

namespace WebApi.Services.Stores;

public record CreateStore : IRequest<Store>
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Category { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerDocument { get; set; }
    public StoreAddress? Address { get; set; }
    public Subscription? Subscription { get; set; }
    public User? User { get; set; }
}

public class CreateStoreHandler : IRequestHandler<CreateStore, Store>
{
    private readonly AppDbContext _context;
    // private readonly IQueue _queue;

    // public CreateStoreHandler(AppDbContext context, IQueue queue)
    // {
    //     _context = context;
    //      _queue = queue;
    // }
    public CreateStoreHandler(AppDbContext context)
    {
        _context = context;
        // _queue = queue;
    }
    

    public async Task<Store> Handle(CreateStore request, CancellationToken cancellationToken)
    {
        var store = new Store(request.Name, request.Phone, request.OwnerName, request.OwnerDocument,
            request.Subscription?.ReferralId, request.Subscription?.Amount ?? 0)
        {
            Category = request.Category,
            User = new User(request.User?.Email, request.User?.Password, request.User?.PublicIp),
            Address = new StoreAddress(request.Address?.Zipcode, request.Address?.State, request.Address?.City,
                request.Address?.Neighborhood, request.Address?.Line1, request.Address?.Number, request.Address?.Line2)
        };

        store.User.ExternalId = store.ExternalId;

        store.Announcements?.Add(new Announcement
        {
            Title = "\ud83c\udf89 Estamos muito animados por tê-lo a bordo na nossa plataforma.",
            Description =
                $"Olá, {store.OwnerName}!<br><br>\ud83c\udf89 Seja muito bem-vindo(a) à Quantso! \ud83c\udf89 <br><br>Estamos felizes demais por você ter escolhido a gente. Queremos que você se sinta completamente à vontade aqui, afinal, criamos a Quantso pensando em oferecer uma plataforma simples e prática para que você tenha mais agilidade nas suas vendas.<br><br>Sabemos que novos começos podem trazer algumas dúvidas. Mesmo acreditando na simplicidade e intuitividade de nossa plataforma, preparamos um vídeo introdutório para você. Esperamos que ele te ajude a ficar por dentro de todas as possibilidades dentro do nosso app.<br><br>Pronto para iniciar essa nova fase conosco?<br><br>Estamos aqui para o que você precisar.",
            Type = 1,
            Status = 1,
            CreatedAt = DateTime.Now
        });

        _context.Stores.Add(store);

        await _context.SaveChangesAsync(cancellationToken);
        
        // await _queue.CreateQueueAsync(store.ExternalId ?? string.Empty);

        return store;
    }
}