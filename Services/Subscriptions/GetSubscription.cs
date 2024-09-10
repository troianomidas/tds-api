using WebApi.Domain.Entities.Subscriptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Subscriptions;

public class GetSubscription : IRequest<Subscription?>
{
    public int StoreId { get; set; }
}

public class GetSubscriptionHandler : IRequestHandler<GetSubscription, Subscription?>
{
    private readonly AppDbContext _dbContext;

    public GetSubscriptionHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Subscription?> Handle(GetSubscription request, CancellationToken cancellationToken)
    {
        return await _dbContext.Subscriptions
            .Where(x => x.StoreId == request.StoreId)
            .Include(x=> x.Billings)
            .FirstOrDefaultAsync(cancellationToken);
    }
}