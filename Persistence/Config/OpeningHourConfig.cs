using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities.Stores;

namespace WebApi.Persistence.Config;

public class OpeningHourConfig : IEntityTypeConfiguration<OpeningHour>
{
    public void Configure(EntityTypeBuilder<OpeningHour> builder)
    {
        builder.ToTable("opening_hours");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}