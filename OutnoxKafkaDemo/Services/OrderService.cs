using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OutnoxKafkaDemo.DataAccess.Contexts;
using OutnoxKafkaDemo.DataAccess.Entities;

namespace OutnoxKafkaDemo.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await Task.FromResult(_context.Orders.ToList<Order>());
    }

    public async Task<Order> GetOrderAsync(int Id)
    {
        return await Task.FromResult(
            _context.Orders.FirstOrDefault(x => x.Order_Id == Id));
    }

    public async Task CreateOrderAsync(Order order)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Orders.Add(order);
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Order] ON;");
            await _context.SaveChangesAsync();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Order] OFF;");
            // await _context.SaveChangesAsync();

            var outboxMessage = new OutboxMessage
            {
                Event_Payload = JsonSerializer.Serialize(order),
                Event_Date = DateTime.Now,
                IsMessageDispatched = false
            };

            _context.OutboxMessages.Add(outboxMessage);

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