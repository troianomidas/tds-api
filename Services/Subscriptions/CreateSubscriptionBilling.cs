using WebApi.Domain.Entities.Subscriptions;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Subscriptions;

public class CreateSubscriptionBilling : IRequest<bool>
{
    public int StoreId { get; set; }
    public int ChargeId { get; set; }
    public decimal Amount { get; set; }
    public string? BilletLink { get; set; }
    public DateTime DueDate { get; set; }
}

public class CreateSubscriptionBillingHandler : IRequestHandler<CreateSubscriptionBilling, bool>
{
    private readonly AppDbContext _dbContext;

    public CreateSubscriptionBillingHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(CreateSubscriptionBilling request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _dbContext.Subscriptions.Where(x => x.StoreId == request.StoreId)
            .Include(x => x.Billings)
            .FirstOrDefaultAsync(cancellationToken);

        if (subscription == null)
            throw new InvalidOperationException();

        subscription.Billings ??= new List<SubscriptionBilling>();

        var billing = new SubscriptionBilling(subscription.Id, request.ChargeId, request.BilletLink, request.DueDate)
        {
            Amount = request.Amount
        };

        subscription.Billings.Add(billing);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}