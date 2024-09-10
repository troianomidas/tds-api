using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class StorePaymentMethodConfig : IEntityTypeConfiguration<StorePaymentMethod>
{
    public void Configure(EntityTypeBuilder<StorePaymentMethod> builder)
    {
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}