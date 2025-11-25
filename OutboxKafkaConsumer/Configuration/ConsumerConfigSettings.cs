using Confluent.Kafka;

namespace OutboxKafkaConsumer.Configuration;

public class ConsumerConfigSettings
{
    public static string Key => nameof(ConsumerConfigSettings);

    public string BootstrapServers { get; set; } = null!;

    public string GroupId { get; set; } = null!;

    public AutoOffsetReset AutoOffsetReset { get; set; }
}