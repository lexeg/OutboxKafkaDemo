using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OutboxKafka.DataAccess.Entities;
using OutboxKafka.DataAccess.Repositories;
using OutboxKafkaDemo.Configuration;

namespace OutboxKafkaDemo.Infrastructure;

public class KafkaProducer : IKafkaProducer
{
    private readonly ProducerConfig _producerConfig;
    private readonly IOutboxMessageRepository _outboxRepository;
    private const string Topic = "test";

    public KafkaProducer(IOutboxMessageRepository outboxRepository, IOptions<ProducerConfigSettings> options)
    {
        _outboxRepository = outboxRepository;

        _producerConfig = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            ClientId = options.Value.ClientId
        };

        _outboxRepository = outboxRepository;
    }

    public async Task SendMessageToKafkaAsync(OutboxMessageEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

        try
        {
            var result = await producer.ProduceAsync
            (Topic, new Message<Null, string>
            {
                Value = entity.Payload
            });

            if (result.Status == PersistenceStatus.Persisted)
            {
                await _outboxRepository.UpdateAsync(entity, true);
            }
        }
        catch (Exception)
        {
            await _outboxRepository.UpdateAsync(entity, false);
        }
    }
}