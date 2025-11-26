using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Configurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("orders");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        builder.Property(e => e.CustomerId)
            .HasColumnName("customer_id");
        builder.Property(e => e.Date)
            .HasColumnName("order_date");
        builder.Property(e => e.Amount)
            .HasColumnName("order_amount");
    }
}