using System.Text.Json;
using Confluent.Kafka;
using OutboxKafka.Common;

namespace OutboxKafkaConsumer;

public class KafkaConsumer : IKafkaConsumer
{
    private const string Topic = "test";
    private const string GroupId = "test_group";
    private const string BootstrapServers = "localhost:9092";

    public Task ConsumeMessagesAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = GroupId,
            BootstrapServers = BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
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