using System.Text.Json;
using Confluent.Kafka;
using OutboxKafka.Common;

namespace OutboxKafkaConsumer;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly string topic = "test";
    private readonly string groupId = "test_group";
    private readonly string bootstrapServers = "localhost:9092";

    public async Task ConsumeMessagesAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = groupId,
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        try
        {
            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);
                        var order = JsonSerializer.Deserialize<Order>(consumer.Message.Value);
                        Console.WriteLine($"Order: {order.OrderId}, {order.CustomerId}, {order.OrderDate}, {order.OrderDate}");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
        }
        catch
        {
            throw;
        }
    }
}