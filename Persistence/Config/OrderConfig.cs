using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasMany(e => e.Items).WithOne(x=> x.Order).HasForeignKey(x => x.OrderId);

        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}