using Microsoft.EntityFrameworkCore;
using OutboxKafka.DataAccess.Configurations;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafka.DataAccess.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<OutboxMessageEntity> OutboxMessages { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }
}