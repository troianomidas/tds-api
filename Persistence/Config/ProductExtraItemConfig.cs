using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ProductExtraItemConfig : IEntityTypeConfiguration<ProductExtraItem>
{
    public void Configure(EntityTypeBuilder<ProductExtraItem> builder)
    {
        builder.ToTable("product_extra_items");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}