using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities.Tickets.TicketAnswers;

namespace WebApi.Persistence.Config;

public class TicketAnswerConfig : IEntityTypeConfiguration<TicketAnswer>
{
    public void Configure(EntityTypeBuilder<TicketAnswer> builder)
    {
        builder.ToTable("ticket_answers");

        builder.Property(a => a.CreatedAt).HasDefaultValueSql("now()");
    }
}