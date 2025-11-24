namespace OutboxKafkaConsumer.BackgroundWorks;

public class KafkaMessageProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IKafkaConsumer _consumer;

    public KafkaMessageProcessor(IServiceScopeFactory scopeFactory, IKafkaConsumer consumer)
    {
        _scopeFactory = scopeFactory;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await _consumer.ConsumeMessagesAsync(cancellationToken);
        }
    }
}