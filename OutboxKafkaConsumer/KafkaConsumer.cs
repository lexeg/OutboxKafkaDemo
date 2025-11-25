using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OutboxKafka.Common;
using OutboxKafkaConsumer.Configuration;

namespace OutboxKafkaConsumer;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly ConsumerConfigSettings _consumerConfigSettings;
    private const string Topic = "test";

    public KafkaConsumer(IOptions<ConsumerConfigSettings> options)
    {
        _consumerConfigSettings = options.Value;
    }

    public Task ConsumeMessagesAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = _consumerConfigSettings.GroupId,
            BootstrapServers = _consumerConfigSettings.BootstrapServers,
            AutoOffsetReset = _consumerConfigSettings.AutoOffsetReset
        };

        using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
        consumerBuilder.Subscribe(Topic);
        var cancelToken = new CancellationTokenSource();

        try
        {
            while (true)
            {
                var consumer = consumerBuilder.Consume(cancelToken.Token);
                var order = JsonSerializer.Deserialize<Order>(consumer.Message.Value);
                Console.WriteLine(
                    $"Order: {order.OrderId}, {order.CustomerId}, {order.OrderDate}, {order.OrderDate}");
            }
        }
        catch (OperationCanceledException)
        {
            consumerBuilder.Close();
        }

        return Task.CompletedTask;
    }
}