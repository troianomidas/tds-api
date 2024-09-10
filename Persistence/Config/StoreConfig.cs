using WebApi.Domain.Entities.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Config;

public class StoreConfig : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}