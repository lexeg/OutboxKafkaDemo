using OutboxKafka.DataAccess.Entities;

namespace OutboxKafkaDemo.Infrastructure;

public interface IKafkaProducer
{
    Task SendMessageToKafkaAsync(OutboxMessage message);
}