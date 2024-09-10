using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class RewardSystemConfig : IEntityTypeConfiguration<RewardSystem>
{
    public void Configure(EntityTypeBuilder<RewardSystem> builder)
    {
        builder.ToTable("rewards_system");

        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}