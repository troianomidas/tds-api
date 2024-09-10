using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("now()");

        builder.HasMany(e => e.Availabilities)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

        builder.HasMany(e => e.ProductExtraMatches)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
    }
}