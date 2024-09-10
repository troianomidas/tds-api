using WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Persistence.Config;

public class WhatsappVerificationConfig : IEntityTypeConfiguration<WhatsappVerification>
{
    public void Configure(EntityTypeBuilder<WhatsappVerification> builder)
    {
        builder.ToTable("whatsapp_verification");
    }
}