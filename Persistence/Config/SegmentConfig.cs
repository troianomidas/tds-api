using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class SegmentsConfig : IEntityTypeConfiguration<Segment>
{
    public void Configure(EntityTypeBuilder<Segment> builder)
    {
        builder.ToTable("segments");

        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}