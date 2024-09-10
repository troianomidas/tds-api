using WebApi.Domain.Constants;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Subscriptions;

public record SubscriptionExpireValidation : IRequest<bool>
{
    
}

public class SubscriptionExpireValidationHandler : IRequestHandler<SubscriptionExpireValidation, bool>
{
    private readonly AppDbContext _context;

    public SubscriptionExpireValidationHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SubscriptionExpireValidation request,
        CancellationToken cancellationToken)
    {
        // List<Store> stores = await _context.Stores
        //     .Include(x=> x.Subscriptions)
        //     .Where(x=> x.Status == StoreStatusConst.Active)
        //     .ToListAsync(cancellationToken);
        //
        // foreach (Store store in stores)
        // {
        //     foreach (var subscription in store.Subscriptions)
        //     {
        //         // (string? status, DateTime? nextExecution, DateTime? nextExpireAt) subscriptionStatus = await _subscription.GetStatusAsync(subscription.PlanId);
        //         // if (subscriptionStatus.status == "new" && subscription.CreatedAt.AddDays(-7) < DateTime.Now)
        //         // {
        //         //     subscription.Status = StoreStatusConst.Expired;
        //         //     store.Status = StoreStatusConst.Expired;
        //         // }
        //     }
        // }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}