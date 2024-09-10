using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ShippingAddressConfig : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.ToTable("order_shipping_addresses");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}