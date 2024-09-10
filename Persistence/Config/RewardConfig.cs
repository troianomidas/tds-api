using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class RewardConfig : IEntityTypeConfiguration<Reward>
{
    public void Configure(EntityTypeBuilder<Reward> builder)
    {
        builder.ToTable("rewards");
        
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}