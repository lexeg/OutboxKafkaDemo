using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Configurations;

public class OutboxMessageEntityConfiguration : IEntityTypeConfiguration<OutboxMessageEntity>
{
    public void Configure(EntityTypeBuilder<OutboxMessageEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("outbox_messages");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("event_id");
        builder.Property(e => e.Payload)
            .HasColumnName("event_payload");
        builder.Property(e => e.Date)
            .HasColumnName("event_date");
        builder.Property(e => e.IsMessageDispatched)
            .HasColumnName("is_message_dispatched");
    }
}