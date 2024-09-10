using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class CustomerBalancePointsViewConfig : IEntityTypeConfiguration<CustomerBalancePointsView>
{
    public void Configure(EntityTypeBuilder<CustomerBalancePointsView> builder)
    {
        builder.ToView("view_customer_balance_points");

        builder.HasNoKey();
    }
}