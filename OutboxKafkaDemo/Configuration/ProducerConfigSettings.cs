namespace OutboxKafkaDemo.Configuration;

public class ProducerConfigSettings
{
    public static string Key => nameof(ProducerConfigSettings);

    public string BootstrapServers { get; set; } = null!;

    public string ClientId { get; set; } = null!;
}