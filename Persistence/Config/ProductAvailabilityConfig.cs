using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ProductAvailabilityConfig : IEntityTypeConfiguration<ProductAvailability>
{
    public void Configure(EntityTypeBuilder<ProductAvailability> builder)
    {
        builder.ToTable("product_availabilities");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}