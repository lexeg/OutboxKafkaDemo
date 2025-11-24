namespace OutboxKafkaConsumer;

public interface IKafkaConsumer
{
    public Task ConsumeMessagesAsync(CancellationToken cancellationToken);
}