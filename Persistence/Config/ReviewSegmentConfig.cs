using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ReviewSegmentsConfig : IEntityTypeConfiguration<ReviewSegment>
{
    public void Configure(EntityTypeBuilder<ReviewSegment> builder)
    {
        builder.ToTable("review_segments");

        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}