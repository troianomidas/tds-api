using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class SubscriptionBillingConfig : IEntityTypeConfiguration<SubscriptionBilling>
{
    public void Configure(EntityTypeBuilder<SubscriptionBilling> builder)
    {
        builder.ToTable("subscription_billings");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}