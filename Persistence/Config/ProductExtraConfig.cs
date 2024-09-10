using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ProductExtraConfig : IEntityTypeConfiguration<ProductExtra>
{
    public void Configure(EntityTypeBuilder<ProductExtra> builder)
    {
        builder.ToTable("product_extras");
        builder.HasMany(e => e.Items).WithOne(x=> x.ProductExtra).HasForeignKey(x => x.ProductExtraId);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}