using OutboxKafka.DataAccess.Contexts;
using OutboxKafkaDemo.Infrastructure;

namespace OutboxKafkaDemo.BackgroundJobs;

public class OutboxMessageProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    // private readonly IKafkaProducer _producer;
    private IKafkaProducer _producer;

    public OutboxMessageProcessor(IServiceScopeFactory scopeFactory/*, IKafkaProducer producer*/)
    {
        _scopeFactory = scopeFactory;
        // _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await PublishOutboxMessagesAsync(cancellationToken);
        }
    }

    private async Task PublishOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        _producer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var entities = dbContext.OutboxMessages.Where(om => om.IsMessageDispatched != true).ToList();

        foreach (var entity in entities)
        {
            try
            {
                await _producer.SendMessageToKafkaAsync(entity);

                entity.IsMessageDispatched = true;
                entity.Date = DateTime.UtcNow;

                dbContext.OutboxMessages.Update(entity);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}