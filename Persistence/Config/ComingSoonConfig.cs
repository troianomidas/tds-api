using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ComingSoonConfig : IEntityTypeConfiguration<ComingSoon>
{
    public void Configure(EntityTypeBuilder<ComingSoon> builder)
    {
        builder.ToTable("coming_soon");
        builder.HasKey(c => new { c.Id });
        builder.HasIndex(a => a.Email);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}