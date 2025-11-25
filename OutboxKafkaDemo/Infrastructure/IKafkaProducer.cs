using OutboxKafkaDemo.DataAccess.Entities;

namespace OutboxKafkaDemo.Infrastructure;

public interface IKafkaProducer
{
    Task SendMessageToKafkaAsync(OutboxMessage message);
}