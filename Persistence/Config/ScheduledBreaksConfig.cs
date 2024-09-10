using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities.Stores;

namespace WebApi.Persistence.Config;

public class ScheduledBreaksConfig : IEntityTypeConfiguration<ScheduledBreak>
{
    public void Configure(EntityTypeBuilder<ScheduledBreak> builder)
    {
        builder.ToTable("scheduled_breaks");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}