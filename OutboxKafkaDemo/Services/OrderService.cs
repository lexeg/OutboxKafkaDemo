using System.Text.Json;
using OutboxKafka.DataAccess.Contexts;
using OutboxKafka.DataAccess.Entities;

namespace OutboxKafkaDemo.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderEntity>> GetAllOrdersAsync()
    {
        return await Task.FromResult(_context.Orders.ToList());
    }

    public async Task<OrderEntity> GetOrderAsync(int id)
    {
        return await Task.FromResult(_context.Orders.FirstOrDefault(x => x.Id == id));
    }

    public async Task CreateOrderAsync(OrderEntity entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
            

            var outboxMessageEntity = new OutboxMessageEntity
            {                
                Id = entity.Id,
                Payload = JsonSerializer.Serialize(entity),
                Date = DateTime.Now,
                IsMessageDispatched = false
            };

            _context.OutboxMessages.Add(outboxMessageEntity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}