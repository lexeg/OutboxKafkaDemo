using OutnoxKafkaDemo.DataAccess.Entities;

namespace OutnoxKafkaDemo.Infrastructure;

public interface IKafkaProducer
{
    Task SendMessageToKafkaAsync(OutboxMessage message);
}