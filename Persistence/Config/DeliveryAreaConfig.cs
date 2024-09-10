using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities.Stores;

namespace WebApi.Persistence.Config;

public class DeliveryAreaConfig : IEntityTypeConfiguration<DeliveryArea>
{
    public void Configure(EntityTypeBuilder<DeliveryArea> builder)
    {
        builder.ToTable("delivery_areas");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}