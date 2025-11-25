using Microsoft.AspNetCore.Mvc;
using OutboxKafkaDemo.DataAccess.Entities;
using OutboxKafkaDemo.Services;

namespace OutboxKafkaDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("GetOrders")]
    public async Task<List<Order>> GetOrders()
    {
        return await _orderService.GetAllOrdersAsync();
    }

    [HttpGet("{id}")]
    public async Task<Order> GetOrder(int id)
    {
        return await _orderService.GetOrderAsync(id);
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        await _orderService.CreateOrderAsync(order);
        return Ok();
    }
}