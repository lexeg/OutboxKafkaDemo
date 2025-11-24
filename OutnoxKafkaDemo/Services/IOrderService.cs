using OutnoxKafkaDemo.DataAccess.Entities;

namespace OutnoxKafkaDemo.Services;

public interface IOrderService
{
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<Order> GetOrderAsync(int Id);
    public Task CreateOrderAsync(Order order);
}