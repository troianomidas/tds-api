using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ProductExtraMatchConfig : IEntityTypeConfiguration<ProductExtraMatch>
{
    public void Configure(EntityTypeBuilder<ProductExtraMatch> builder)
    {
        builder.ToTable("product_extra_matches");
        builder.HasOne(e => e.ProductExtra).WithMany(x=> x.ProductExtraMatches).HasForeignKey(x => x.ProductExtraId);
        builder.HasOne(e => e.Product).WithMany(x=> x.ProductExtraMatches).HasForeignKey(x => x.ProductId);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}