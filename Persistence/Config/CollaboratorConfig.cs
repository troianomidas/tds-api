using WebApi.Domain.Entities.Collaborators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class CollaboratorConfig : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.ToTable("collaborators");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}