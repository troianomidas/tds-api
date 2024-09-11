using System.Reflection;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Collaborators;
using WebApi.Domain.Entities.Products;
using WebApi.Domain.Entities.Stores;
using WebApi.Domain.Entities.Subscriptions;
using WebApi.Domain.Entities.Tickets;
using WebApi.Domain.Entities.Tickets.TicketAnswers;
using Microsoft.EntityFrameworkCore;
namespace WebApi.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();

    public DbSet<Store> Stores => Set<Store>();
    public DbSet<StoreDelivery> StoreDeliveries => Set<StoreDelivery>();
    public DbSet<DeliveryArea> DeliveryAreas => Set<DeliveryArea>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> Categories => Set<ProductCategory>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<ReferralCode> ReferralCodes => Set<ReferralCode>();
    public DbSet<ProductExtra> ProductExtras => Set<ProductExtra>();
    public DbSet<ProductExtraItem> ProductExtraItems => Set<ProductExtraItem>();
    public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Reward> Rewards => Set<Reward>();
    public DbSet<RewardSystem> RewardsSystem => Set<RewardSystem>();
    public DbSet<CustomerBalancePointsView> CustomerBalancePointsVw => Set<CustomerBalancePointsView>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<StorePaymentMethod> StorePaymentMethods => Set<StorePaymentMethod>();
    public DbSet<OpeningHour> OpeningHours => Set<OpeningHour>();
    public DbSet<ScheduledBreak> ScheduledBreaks => Set<ScheduledBreak>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionBilling> SubscriptionBillings => Set<SubscriptionBilling>();
    public DbSet<ExpiratedPointsView> ExpiratedPointsVw => Set<ExpiratedPointsView>();
    public DbSet<Collaborator> Collaborators => Set<Collaborator>();
    public DbSet<ComingSoon> ComingSoon => Set<ComingSoon>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketAnswer> TicketAnswers => Set<TicketAnswer>();
    public DbSet<StoreAddress> StoreAddresses => Set<StoreAddress>();
    public DbSet<StoreSettings> StoreSettings => Set<StoreSettings>();
    public DbSet<VwProductHighlights> VwProductHighlights => Set<VwProductHighlights>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}