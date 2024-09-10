using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class StoreDeliveryConfig : IEntityTypeConfiguration<StoreDelivery>
{
    public void Configure(EntityTypeBuilder<StoreDelivery> builder)
    {
        builder.ToTable("store_deliveries");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}