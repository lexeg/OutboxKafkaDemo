using Microsoft.EntityFrameworkCore;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("Outbox_Message");
            entity.HasKey(e => e.Id);
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<Order> Orders { get; set; }
}