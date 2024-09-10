using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class ExpiratedPointsViewConfig : IEntityTypeConfiguration<ExpiratedPointsView>
{
    public void Configure(EntityTypeBuilder<ExpiratedPointsView> builder)
    {
        builder.ToView("view_expirated_points");

        builder.HasNoKey();
    }
}