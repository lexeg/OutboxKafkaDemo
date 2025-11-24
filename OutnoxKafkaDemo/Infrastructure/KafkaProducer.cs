using System.Net;
using Confluent.Kafka;
using OutnoxKafkaDemo.DataAccess.Entities;
using OutnoxKafkaDemo.DataAccess.Repositories;

namespace OutnoxKafkaDemo.Infrastructure;

public class KafkaProducer : IKafkaProducer
{
    private readonly ProducerConfig _producerConfig;
    private readonly IOutboxMessageRepository _outboxRepository;
    private readonly string topic = "test";

    public KafkaProducer(IOutboxMessageRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;

        _producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092", 
            ClientId = Dns.GetHostName()
        };

        _outboxRepository = outboxRepository;
    }

    public async Task SendMessageToKafkaAsync(OutboxMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

        try
        {
            var result = await producer.ProduceAsync
            (topic, new Message<Null, string>
            {
                Value = message.Event_Payload
            });

            if (result.Status == PersistenceStatus.Persisted)
            {
                await _outboxRepository.UpdateAsync(message, true);
            }
        }
        catch (Exception)
        {
            await _outboxRepository.UpdateAsync(message, false);
        }
    }
}