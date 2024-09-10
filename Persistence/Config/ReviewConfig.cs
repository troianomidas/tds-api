using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasMany(e => e.ReviewSegments).WithOne(x=> x.Review).HasForeignKey(x => x.ReviewId);

        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}