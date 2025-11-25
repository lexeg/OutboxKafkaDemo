using OutboxKafkaDemo.DataAccess.Entities;

namespace OutboxKafkaDemo.Services;

public interface IOrderService
{
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<Order> GetOrderAsync(int Id);
    public Task CreateOrderAsync(Order order);
}