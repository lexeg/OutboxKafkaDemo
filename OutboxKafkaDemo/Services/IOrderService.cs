using OutboxKafka.DataAccess.Entities;

namespace OutboxKafkaDemo.Services;

public interface IOrderService
{
    public Task<List<OrderEntity>> GetAllOrdersAsync();
    public Task<OrderEntity> GetOrderAsync(int Id);
    public Task CreateOrderAsync(OrderEntity entity);
}