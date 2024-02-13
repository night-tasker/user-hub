using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Infrastructure.Persistence.Outbox;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(x => x.OccurredOn)
            .IsRequired();
        
        builder.Property(x => x.ProcessedOn)
            .IsRequired(false);
        
        builder.Property(x => x.IsProcessed);
        
        builder.Property(x => x.Error)
            .IsRequired(false)
            .HasColumnType("jsonb");
    }
}